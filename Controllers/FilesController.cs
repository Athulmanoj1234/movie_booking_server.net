using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using movie_booking.Dtos.Request;
using movie_booking.Dtos.Response;
using movie_booking.services;

namespace movie_booking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Files : ControllerBase
    {
        private FileUploadService FileUploadService;

        public Files(FileUploadService fileUploadService)
        {
            this.FileUploadService = fileUploadService;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> FileUploadIssue(List<FileUploadDto> FileUpload)
        {
            var result = await this.FileUploadService.AddFileMetadata(FileUpload);

            return Ok(result);
        }

        [HttpGet("virustest")]
        //Http
        public async Task<IActionResult> UploadedFileScannedDetails([FromQuery] string FileName) {
            var result = await this.FileUploadService.UploadedFileScannedDetails(FileName);
            return Ok(result);
        }
    }
}
