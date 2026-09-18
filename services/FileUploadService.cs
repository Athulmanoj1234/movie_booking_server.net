using Microsoft.EntityFrameworkCore;
using movie_booking.data;
using movie_booking.Dtos.Request;
using movie_booking.Dtos.Response;
using movie_booking.Dtos.Response.FileUpload;
using movie_booking.Models.FileUpload;
using movie_booking.SmartEnums;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace movie_booking.services
{
    public class FileUploadService
    {
        private ApplicationDbContext _dbContext;
        private readonly R2StorageService _r2StorageService;
        private readonly ClamAvScanner _clamAvScanner;
        private readonly string _buckerName;
        public FileUploadService(ApplicationDbContext DbContext, R2StorageService R2StorageService, ClamAvScanner clamAvScanner, IConfiguration Configuration)
        {
            this._dbContext = DbContext;
            this._r2StorageService = R2StorageService;
            this._clamAvScanner = clamAvScanner;
            this._buckerName = Configuration["R2:BucketName"] ?? "my-bucket";
        }

        public async Task<SuccessOrErrorResponseDto<FileUploadResponseVm>> AddFileMetadata(List<FileUploadDto> FileUpload)
        {
            // Method body will be implemented later
            try
            {
                List<FileMeta> createdFileElements = new List<FileMeta>();
                foreach (FileUploadDto file in FileUpload)
                {
                    var fileContentType = FileContentType.FromKey(file.FileContentType.ToString()).Value;
                    var fileId = Guid.NewGuid();
                    var fileElement = new FileMeta()
                    {
                        Id = fileId,
                        FileName = file.FileName,
                        FileSize = file.FileSize,
                        FileContenType = fileContentType,
                        FileUploadStatus = FileUploadStatuses.Uploading,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                    };
                    createdFileElements.Add(fileElement);
                }
                await this._dbContext.AddRangeAsync(createdFileElements);
                await this._dbContext.SaveChangesAsync();

                var createdFiles = createdFileElements.Select(cfe => new FilesResponseVm
                {
                    FileId = cfe.Id,
                    FileName = cfe.FileName,
                    FileUploadStatus = FileUploadStatuses.Uploading.ToString(),
                    //FileContentType = FileContentType.FromKey(cfe.FileContenType.ToString()).Value,
                    PresignedUrl = this._r2StorageService.GetPresignedUploadUrl(cfe.FileName, FileContentType.FromValue(cfe.FileContenType).Value)
                }).ToList();

                return new SuccessOrErrorResponseDto<FileUploadResponseVm>()
                {
                    StatusCode = 200,
                    Messege = "Added file meta data successfully",
                    Data = new FileUploadResponseVm {
                        AddedFiles = createdFiles,
                    }
                };
            }
            catch (Exception ex) {
                return new SuccessOrErrorResponseDto<FileUploadResponseVm>()
                {
                    StatusCode = 500,
                    Messege = ex.Message,
                };
            }
        }

        public async Task<SuccessOrErrorResponseDto<FileMeta>> UploadedFileScannedDetails(Guid FileId, string FileName)
        {
            try
            {
                var fileDetails = await this._dbContext.FileMeta.FirstOrDefaultAsync(fm => fm.Id == FileId);

                //1-> instanstiate a new connection and make available it preffered host here we are make available in localhost 
                var factory = new ConnectionFactory { HostName = "Localhost" };
                //after instantialting a connection factorr we need to create a new connection by creating new connection with createConnectionAsync() we are making connection with new rabbitmq broker
                using var connection = await factory.CreateConnectionAsync();
                //to communicate with the broker instance we need to create a new channel
                using var channel = await connection.CreateChannelAsync();

                //decalare a queue to send messages
                await channel.QueueDeclareAsync(
                    queue: "file-process", // the name of teh queue
                    durable: true, // if the messages in the queue should be last even after the broker or application stops
                    exclusive: false,  //if the queue should be eclusive to the connection that uses 
                    autoDelete: false, //queue should be deleted if the subsriber unsubsibes
                    arguments: null);

                var objectData = new
                {
                    EventId = Guid.NewGuid(),
                    EventType = "fileUploadProcessing",
                    FileDetails = new
                    {
                        FileId = fileDetails.Id,
                        FileName = fileDetails.FileName,
                        FileContentype = fileDetails.FileContenType,
                        FileUploadStatus = fileDetails.FileUploadStatus,
                    }
                };

                string jsonString = JsonSerializer.Serialize(objectData);
                byte[] body = Encoding.UTF8.GetBytes(jsonString);

                // we can now use the channel instance to do the basic publish
                await channel.BasicPublishAsync(
                    exchange: string.Empty,
                    routingKey: "file-process", //we are routing the publish to the queue that we earlier make
                    mandatory: true, // to push mandatorily use this queue named messages
                    basicProperties: new BasicProperties { Persistent = true },
                    body: body); // what data or what messages need to sent 

                return new SuccessOrErrorResponseDto<FileMeta>
                {
                    StatusCode = 200,
                    Messege = "message added to queue successfully"
                };

                //Stream fileStream = await this._r2StorageService.GetObjectFromR2(FileName);
                //ClamAvScannedResponse scannedResponse = await this._clamAvScanner.ScanFileAsync(fileStream);

                //if (!scannedResponse.IsDefaultFound && scannedResponse.IsScanningCompleted)
                //{
                //    return new SuccessOrErrorResponseDto<FileMeta>()
                //    {
                //        StatusCode = 200,
                //        Messege = scannedResponse.ScannedMessage,
                //    };
                //}
                //else if (scannedResponse.IsDefaultFound)
                //{
                //    return new SuccessOrErrorResponseDto<FileMeta>()
                //    {
                //        StatusCode = 400,
                //        Messege = scannedResponse.ScannedMessage,
                //    };
                //}
                //else if (!scannedResponse.IsDefaultFound && !scannedResponse.IsScanningCompleted)
                //{
                //    return new SuccessOrErrorResponseDto<FileMeta>()
                //    {
                //        StatusCode = 500,
                //        Messege = scannedResponse.ScannedMessage,
                //    };
                //}
                //else {
                //    return new SuccessOrErrorResponseDto<FileMeta>()
                //    {
                //        StatusCode = 500,
                //        Messege = "something wrong in the file scanning",
                //    };
                //}

            }
            catch (Exception ex)
            {
                return new SuccessOrErrorResponseDto<FileMeta>()
                {
                    StatusCode = 500,
                    Messege = ex.Message,
                };
            }
        }
    }

}
