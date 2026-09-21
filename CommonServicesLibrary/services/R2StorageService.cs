using Amazon.S3;
using Amazon.S3.Model;
using System.Threading.Tasks;

namespace CommonServicesLibrary.services
{
    public class R2StorageService
    {
        private readonly IAmazonS3 _s3Client;
        private readonly string _bucketName;

        public R2StorageService(IAmazonS3 s3Client)
        {
            _s3Client = s3Client;
            _bucketName = "app-moviebooking-data" ?? "my-bucket";
        }

        // for downloading private file we use need presigned download url 
        public string GetPresignedDownloadUrl(string FileName) {
            var request = new GetPreSignedUrlRequest()
            {
                BucketName = this._bucketName,
                Key = FileName,
                Verb = HttpVerb.GET,
                Expires = DateTime.UtcNow.AddHours(1)
            };
            var url = this._s3Client.GetPreSignedURL(request);
            return url;
        }

        public string GetPresignedUploadUrl(string FileName, string FileContentType) {
            var request = new GetPreSignedUrlRequest()
            {
                // first specify the bucket name
                BucketName = this._bucketName,
                // key should be given ie the object file location in the r2 object storage bucket
                Key = FileName,
                // put means replace or add the resource under the specified file name
                Verb = HttpVerb.PUT,
                //expiry of the presigned url
                Expires = DateTime.UtcNow.AddMinutes(15),
                // content type of the file same contentype also should be used in the frontend when uploading the file also
                ContentType = FileContentType
            };

            var url = this._s3Client.GetPreSignedURL(request);
            return url;
        }

        public async Task<Stream> GetObjectFromR2(string FileName) {

            // first need to create request for getting the object from the r2 object storage using bucket name and file name
            var request = new GetObjectRequest()
            {
                BucketName = this._bucketName,
                Key = FileName
            };
            // get file response from the s3 client object request
            var objectRequestResponse = await this._s3Client.GetObjectAsync(request);
            // returns streaming object - is an object that gives you access to the body/ data of the R2 response.
            return objectRequestResponse.ResponseStream;
        }

    }
}
