using CommonServicesLibrary.data;
using CommonServicesLibrary.Models.FileUpload;
using CommonServicesLibrary.services;
using CommonServicesLibrary.Shared;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;


namespace FileProcessingConsumerService1
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public Worker(ILogger<Worker> logger, IDbContextFactory<ApplicationDbContext> contextFactory, IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            this._contextFactory = contextFactory;
            this._serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            while (!stoppingToken.IsCancellationRequested)
            {
                var factory = new ConnectionFactory { HostName = "Localhost" };
                //after instantialting a connection factorr we need to create a new connection by creating new connection with createConnectionAsync() we are making connection with new rabbitmq broker
                using var connection = await factory.CreateConnectionAsync();
                //to communicate with the broker instance we need to create a new channel
                using var channel = await connection.CreateChannelAsync();

                //decalare a queue to send messages
                await channel.QueueDeclareAsync(
                    queue: "file-process", // the name of teh queue
                    durable: true, // if the messages in the queue should be last even after the broker or application stops
                    exclusive: false,  //if the queue should be eclusive to the connection that uses. ie multiple connections cannot access
                    autoDelete: false, //queue should be deleted if the subsriber unsubsibes
                    arguments: null);

                // upto this step same as the producer


                Console.WriteLine("waiting for messages...");

                //define new consumer using the channel
                //AsyncEventingBasicConsumer is a class from the RabbitMQ.Client.
                //It is a message listener that:
                //subscribes to a queue
                //waits for messages
                //triggers an event when a message arrives
                //It works with async event handlers, which is why it is called AsyncEventing.
                var consumer = new AsyncEventingBasicConsumer(channel);

                // next we need to receive the message when receiving the message an event in here "ReceivedAsync" will be fired or invoked so we should use the delegate
                // 1-> we need to receive the message
                // 2-> then message will be in byte form so we need to convert in into original data format like string, int etc
                consumer.ReceivedAsync += async (sender, EventArgs) =>
                {
                    try
                    {
                        byte[] body = EventArgs.Body.ToArray();
                        string message = Encoding.UTF8.GetString(body);

                        Console.WriteLine($"the message received from the producer is - {message}");

                        var objectBody = JsonSerializer.Deserialize<FileDetailProcessing>(message);

                        using (var dbContext = _contextFactory.CreateDbContext())
                        {
                            var fileData = await dbContext.FileMeta.
                                                 FirstOrDefaultAsync(fm => fm.Id == objectBody.FileDetails.FileId);

                            await using (var scope = _serviceScopeFactory.CreateAsyncScope())
                            {
                                Console.WriteLine("created scope factory....");
                                var r2StorageService = scope.ServiceProvider.GetRequiredService<R2StorageService>();
                                var clamAvScanner = scope.ServiceProvider.GetRequiredService<ClamAvScanner>();
                                Stream fileStream = await r2StorageService.GetObjectFromR2(objectBody.FileDetails.FileName);
                                ClamAvScannedResponse scannedResponse = await clamAvScanner.ScanFileAsync(fileStream);
                                Console.WriteLine("completed scanning....");

                                if (!scannedResponse.IsDefaultFound && scannedResponse.IsScanningCompleted)
                                {
                                    fileData.FileUploadStatus = FileUploadStatuses.Uploaded;
                                    Console.WriteLine("The file is scanned successfully.... no errors found");
                                }
                                else if (scannedResponse.IsDefaultFound)
                                {
                                    fileData.FileUploadStatus = FileUploadStatuses.Rejected;
                                    Console.WriteLine("The file is scanned unsuccessfully.... errors found!!!!!!");
                                }
                                else if (!scannedResponse.IsDefaultFound && !scannedResponse.IsScanningCompleted)
                                { }
                                else
                                { }
                            }
                            await dbContext.SaveChangesAsync();
                        }
                    }
                    catch (Exception ex) { 
                        Console.WriteLine(ex.Message);
                    }

                    // now we need to subscribe to the message to remove it from the queue
                    // Since the event system passes it as a generic object, we cast it back to its real type: (AsyncEventingBasicConsumer)sender  now we get access to the properties like channel BasicAckAsync sends an ACK (acknowledgement) to RabbitMQ. eventArgs.DeliveryTag Each message delivered by RabbitMQ has a unique ID called a delivery tag. So this tells RabbitMQ which message we are acknowledging. multiple: false This parameter controls how many messages are acknowledged. if multiple: true then it will acknowledge trhe messsages previoulsy not acknowledged
                    await ((AsyncEventingBasicConsumer)sender).Channel.BasicAckAsync(EventArgs.DeliveryTag, multiple: false);
                };

                // to register the consumer with the queue  here autoAct false because to acknowledge the message manually ie the methods like BasicAckAsync is used 
                await channel.BasicConsumeAsync("file-process", autoAck: false, consumer);

                Console.ReadLine();
            }
        }
    }
}
