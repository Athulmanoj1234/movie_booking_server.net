namespace movie_booking.SmartEnums
{
    public abstract class FileContentType : Enumeration<FileContentType>
    {
        public FileContentType(string key, string value) : base(key, value) { }

        public static readonly FileContentType PDF =
            new PDFContentType();

        public static readonly FileContentType JPEG =
            new JPEGContentType();

        public static readonly FileContentType PNG =
            new PNGContentType();

        public static readonly FileContentType DOC =
            new DOCContentType();

        public static readonly FileContentType DOCX =
            new DOCXContentType();

        public static readonly FileContentType XLS =
            new XLSContentType();

        public static readonly FileContentType XLSX =
            new XLSXContentType();

        public static readonly FileContentType PPT =
            new PPTContentType();

        public static readonly FileContentType PPTX =
            new PPTXContentType();

        public static readonly FileContentType TXT =
            new TXTContentType();

        public static readonly FileContentType CSV =
            new CSVContentType();

        public static readonly FileContentType ZIP =
            new ZIPContentType();

       

        public sealed class PDFContentType : FileContentType
        {
            public PDFContentType()
                : base("pdf", "application/pdf")
            {
            }
        }

        public sealed class JPEGContentType : FileContentType
        {
            public JPEGContentType()
                : base("jpg", "image/jpeg")
            {
            }
        }

        public sealed class PNGContentType : FileContentType
        {
            public PNGContentType()
                : base("png", "image/png")
            {
            }
        }

        public sealed class DOCContentType : FileContentType
        {
            public DOCContentType()
                : base("doc", "application/msword")
            {
            }
        }

        public sealed class DOCXContentType : FileContentType
        {
            public DOCXContentType()
                : base(
                    "docx",
                    "application/vnd.openxmlformats-officedocument.wordprocessingml.document")
            {
            }
        }

        public sealed class XLSContentType : FileContentType
        {
            public XLSContentType()
                : base("xls", "application/vnd.ms-excel")
            {
            }
        }

        public sealed class XLSXContentType : FileContentType
        {
            public XLSXContentType()
                : base(
                    "xlsx",
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
            }
        }

        public sealed class PPTContentType : FileContentType
        {
            public PPTContentType()
                : base("ppt", "application/vnd.ms-powerpoint")
            {
            }
        }

        public sealed class PPTXContentType : FileContentType
        {
            public PPTXContentType()
                : base(
                    "pptx",
                    "application/vnd.openxmlformats-officedocument.presentationml.presentation")
            {
            }
        }

        public sealed class TXTContentType : FileContentType
        {
            public TXTContentType()
                : base("txt", "text/plain")
            {
            }
        }

        public sealed class CSVContentType : FileContentType
        {
            public CSVContentType()
                : base("csv", "text/csv")
            {
            }
        }

        public sealed class ZIPContentType : FileContentType
        {
            public ZIPContentType()
                : base("zip", "application/zip")
            {
            }
        }
    }
}