using movie_booking.Models.FileUpload;

namespace movie_booking.Dtos.Response.FileUpload
{
    public class FileUploadResponseVm
    {
        public List<FilesResponseVm> AddedFiles { get; set; }
        public string PresignedUrl { get; set; }
    }

    public class FilesResponseVm {
        public Guid FileId { get; set; }
        public string FileName { get; set; }
        public string FileUploadStatus { get; set; }
        public string? FileContentType { get; set; }
        public string PresignedUrl { get; set; }
    }
}
