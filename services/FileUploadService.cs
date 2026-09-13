using Microsoft.EntityFrameworkCore;
using movie_booking.data;
using movie_booking.Dtos.Request;
using movie_booking.Dtos.Response;
using movie_booking.Dtos.Response.FileUpload;
using movie_booking.Models.FileUpload;
using movie_booking.SmartEnums;

namespace movie_booking.services
{
    public class FileUploadService
    {
        private ApplicationDbContext _dbContext;
        private readonly R2StorageService _r2StorageService;
        private readonly ClamAvScanner _clamAvScanner;
        public FileUploadService(ApplicationDbContext DbContext, R2StorageService R2StorageService, ClamAvScanner clamAvScanner )
        {
            this._dbContext = DbContext;
            this._r2StorageService = R2StorageService;
            this._clamAvScanner = clamAvScanner;
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

        public async Task<SuccessOrErrorResponseDto<FileMeta>> UploadedFileScannedDetails(string FileName)
        {
            Stream fileStream = await this._r2StorageService.GetObjectFromR2(FileName);
            bool isSecure = await this._clamAvScanner.ScanFileAsync(fileStream);

            if (isSecure) {
                return new SuccessOrErrorResponseDto<FileMeta>()
                {
                    StatusCode = 200,
                    Messege = "NO viruses or malwares in the file"
                };
            }
            return new SuccessOrErrorResponseDto<FileMeta>()
            {
                StatusCode = 400,
                Messege = "file is virused"
            };
        }
    }

}
