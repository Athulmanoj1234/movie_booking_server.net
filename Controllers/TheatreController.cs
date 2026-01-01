using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using movie_booking.Application;
using movie_booking.Dtos.Request.Theatre.FirstLevelUploadDto;
using movie_booking.Dtos.Request.Theatre.SecondLevelUploadDto;
using movie_booking.Dtos.Request.Theatre.ThirdLevelUploadDto;
using movie_booking.Dtos.Response;
using movie_booking.Models;
using movie_booking.Models.Ttheatre;

namespace movie_booking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TheatreController : ControllerBase
    {
        private TheatreBL _theatreBl;
        public TheatreController(TheatreBL TheatreBl) {
            this._theatreBl = TheatreBl;
        }

        [HttpPost("firstLevelOnboard")]
        public async Task<IActionResult> FirstLevelTheatreOnBoard(FirstLevelTheaterInfoDto FirstLevelUploadDto) {
            SuccessOrErrorResponseDto<TheatreInfo> response = await this._theatreBl.FirstLevelTheatreOnBoard(FirstLevelUploadDto);

            if (response.StatusCode >= 400 && response.StatusCode < 500) return BadRequest(response.Messege);
            if (response.StatusCode >= 500 && response.StatusCode < 600)
                return StatusCode(StatusCodes.Status500InternalServerError, response.Messege);

            return Ok(response);
        }

        [HttpPost("secondLevelOnboard")]
        public async Task<IActionResult> SecondLevelTheatreOnBoard(TheatreLocationDto SecondLevelUploadDto)
        {
            SuccessOrErrorResponseDto<TheatreLocation> response = await this._theatreBl.SecondLevelTheatreOnBoard(SecondLevelUploadDto);

            if (response.StatusCode >= 400 && response.StatusCode < 500) return BadRequest(response.Messege);
            if (response.StatusCode >= 500 && response.StatusCode < 600)
                return StatusCode(StatusCodes.Status500InternalServerError, response.Messege);

            return Ok(response);
        }

        [HttpPost("thirdLevelOnboard")]
        public async Task<IActionResult> ThirdLevelOnboardRowInfoAdd(ScreenRowsDto screenRowsDto)
        {
            SuccessOrErrorResponseDto<TheatreSeat> response = await this._theatreBl.ThirdLevelRowInfoAdd(screenRowsDto);

            if (response.StatusCode >= 400 && response.StatusCode < 500) return BadRequest(response.Messege);
            if (response.StatusCode >= 500 && response.StatusCode < 600)
                return StatusCode(StatusCodes.Status500InternalServerError, response.Messege);

            return Ok(response);
        }
    }
}
