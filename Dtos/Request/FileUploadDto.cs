using System.Text.Json.Serialization;

namespace movie_booking.Dtos.Request
{
    public class FileUploadDto
    {
        public string FileName { get; set; }
        public long FileSize { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public FileContentTypeKey FileContentType { get; set; }
    }

    public enum FileContentTypeKey
    {
        pdf,
        jpg,
        png,
        doc,
        docx,
        xls,
        xlsx,
        ppt,
        pptx,
        txt,
        csv,
        zip
    }
}


