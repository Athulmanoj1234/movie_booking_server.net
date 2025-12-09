using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using movie_booking.Application;
using movie_booking.Dtos.Request;
using movie_booking.Dtos.Response;
using movie_booking.Models;
using movie_booking.services;

namespace movie_booking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieDetailsController : ControllerBase
    {
        private MovieDetailsService _movieDetialsService;
        private MovieDetailBL _movieDetailBL;
        public MovieDetailsController(MovieDetailsService movieDetialsService, MovieDetailBL movieDetailBL) {
            this._movieDetialsService = movieDetialsService;
            this._movieDetailBL = movieDetailBL;
        }

        [HttpPost("Add-Director")]
        public async Task<IActionResult> AddDirectorInfo(DirectorInfoDto directorInfo) {
            SuccessOrErrorResponseDto<DirectorInfo> response = await this._movieDetailBL.AddDirectorInfo(directorInfo);

            if (response.StatusCode >= 400 && response.StatusCode < 500) return BadRequest(response.Messege);
            if (response.StatusCode >= 500 && response.StatusCode < 600)
                return StatusCode(StatusCodes.Status500InternalServerError, response.Messege);

            return Ok(response);
        }


        [HttpPost("Add-Actor")]
        public async Task<IActionResult> AddActorInfo(ActorInfoDto ActorInfo)
        {
            SuccessOrErrorResponseDto<ActorInfo> response = await this._movieDetailBL.AddActorInfo(ActorInfo);

            if (response.StatusCode >= 400 && response.StatusCode < 500) return BadRequest(response.Messege);
            if (response.StatusCode >= 500 && response.StatusCode < 600)
                return StatusCode(StatusCodes.Status500InternalServerError, response.Messege);

            return Ok(response);
        }

        [HttpPost("Add-Writer")]
        public async Task<IActionResult> AddWriterInfo(WriterInfoDto WriterInfo)
        {
            SuccessOrErrorResponseDto<WriterInfo> response = await this._movieDetailBL.AddWriterInfo(WriterInfo);

            if (response.StatusCode >= 400 && response.StatusCode < 500) return BadRequest(response.Messege);
            if (response.StatusCode >= 500 && response.StatusCode < 600)
                return StatusCode(StatusCodes.Status500InternalServerError, response.Messege);

            return Ok(response);
        }

        [HttpPost("Add-MovieInfo")]
        public async Task<IActionResult> AddMovieInfo([FromForm]MovieInfoDto MovieInfo)
        {
            SuccessOrErrorResponseDto<MovieInfo> response = await this._movieDetailBL.AddMovieInfo(MovieInfo);

            if (response.StatusCode >= 400 && response.StatusCode < 500) return BadRequest(response.Messege);
            if (response.StatusCode >= 500 && response.StatusCode < 600)
                return StatusCode(StatusCodes.Status500InternalServerError, response.Messege);

            return Ok(response);
        }

        [HttpGet("MovieInfo/{id}")]
        //[Route("{id: int}")]
        public async Task<IActionResult> GetMovieInfo(int id) {

            SuccessOrErrorResponseDto<MovieInfo> response = await this._movieDetailBL.GetMovieDetails(id);

            if (response.StatusCode >= 400 && response.StatusCode < 500) return BadRequest(response.Messege);
            if (response.StatusCode >= 500 && response.StatusCode < 600)
                return StatusCode(StatusCodes.Status500InternalServerError, response.Messege);

            return Ok(response);
        }

        [HttpPatch("AddDirectorWriterActorToMovieInfo/{id}")]
        public async Task<IActionResult> AddDirectorWriterActorToMovieInfo(int id, DirectorWriterActorUpdateDto DirectorWriterActorUpdateData) {
            SuccessOrErrorResponseDto<DirectorWriterActorUpdateDto> response = await this._movieDetailBL.AddDirectorWriterActorToMovieInfo(id, DirectorWriterActorUpdateData);

            if (response.StatusCode >= 400 && response.StatusCode < 500) return BadRequest(response.Messege);
            if (response.StatusCode >= 500 && response.StatusCode < 600)
                return StatusCode(StatusCodes.Status500InternalServerError, response.Messege);

            return Ok(response);
        }
    }
}
