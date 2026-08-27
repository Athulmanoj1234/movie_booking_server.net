namespace movie_booking.Models.FileUpload
{
    public class FileMeta
    {
        public Guid Id { get; set; }
        public string FileName { get; set; }
        public long FileSize { get; set; }
        public ContenTypes FileContenType { get; set; }
        public FileUploadStatuses FileUploadStatus { get; set; }
        // to identify the files in the object storage we should use object keys like - uploads/abc.jpg - not in the form of url like - https://my-files.s3.amazonaws.com/uploads/abc.jpg ie the url 
        //can get changed but the object keys cannot gets changed ie some times user can access from the object storage and other time user can access from the cdns thats why
        public string ObjectKey { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
    }

    public enum ContenTypes
    {
        ImageJpeg,
        ImagePng,
        ApplicationPdf,
        VideoMp4,
        TextPlain,
    }

    public enum FileUploadStatuses { 
        Uploading,
        Uploaded,
        Completed,
        Cancelled,
        Rejected
    }

}



