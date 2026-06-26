using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

var factory = new ConnectionFactory { HostName = "Localhost" };
using var connection = await factory.CreateConnectionAsync();
using var channel = await connection.CreateChannelAsync();

//first decalre the exchange in which includes the ticket details
await channel.ExchangeDeclareAsync(
    exchange: "Ticket Details", // the name of teh queue
    durable: true, // if the messages in the exchange should be last even after the broker or application stops Exchange will survive server restart
    autoDelete: false, //exchange should be deleted if the subsriber unsubsibes, Exchange will NOT be deleted automatically
                       //    If autoDelete: true
                       //Exchange deleted when:
                       //No queues are bound to it
    type: ExchangeType.Fanout
    );

//decalare a queue to receive messages
await channel.QueueDeclareAsync(
    queue: "Mail Ticket",
    durable: true,
    exclusive: false,
    autoDelete: false,
    arguments: null
    );

//we are binding the queue to Fanout exchange
await channel.QueueBindAsync("Mail Ticket", "Ticket Details", string.Empty);

Console.WriteLine("Waiting For mail ticket details.....");

// for consuming the messages
var consumer = new AsyncEventingBasicConsumer(channel);

consumer.ReceivedAsync += async (sender, EventArgs) =>
{
    byte[] body = EventArgs.Body.ToArray();
    string message = Encoding.UTF8.GetString(body);

    Console.WriteLine($"the message received from the producer is - {message}");

    await ((AsyncEventingBasicConsumer)sender).Channel.BasicAckAsync(EventArgs.DeliveryTag, multiple: false);
};

await channel.BasicConsumeAsync("Mail Ticket", autoAck: false, consumer);

Console.ReadLine();



