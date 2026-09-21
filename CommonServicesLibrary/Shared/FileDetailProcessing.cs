using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonServicesLibrary.Shared
{
   public class FileDetailProcessing
   {
        public Guid EventId { get; set; }
        public string EventType { get; set; }
        public FileDetails FileDetails { get; set; }
    }

    public class FileDetails {
        public Guid FileId { get; set; }
        public string FileName { get; set; }
        public string FileContentype { get; set; }
        public int FileUploadStatus { get; set; }
    }
}
