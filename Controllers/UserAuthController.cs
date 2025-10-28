using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using movie_booking.Application;
using movie_booking.data;
using movie_booking.Dtos.Request;
using movie_booking.Dtos.Response;

namespace movie_booking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAuthController : ControllerBase
    {
        private IConfiguration Congiguration;
        private ApplicationDbContext DbContext;
        public AccountBL AccountBL;
        private string RefreshToken;

        public UserAuthController(IConfiguration configuration, ApplicationDbContext dbContext, AccountBL accountBL) {
            this.Congiguration = configuration;
            this.DbContext = dbContext;
            this.AccountBL = accountBL;
        }

        [HttpGet("GetUserByMobileNumber")]
        public async Task<IActionResult> GetOrAddUserbyMobile([FromQuery] string MobileNumber) {
            //var mobileNumber = MobileNumber;
            var response = await this.AccountBL.GetOrAddUserbyMobileAsync(MobileNumber);

            if (response.StatusCode >= 400 && response.StatusCode < 500) return BadRequest(response.Messege);
            if (response.StatusCode >= 500 && response.StatusCode < 600)
                return StatusCode(StatusCodes.Status500InternalServerError, response.Messege);

            return Ok(response);
        }

        [HttpGet("ValidateOtp")]
        public async Task<IActionResult> ValidateOtp([FromQuery] string MobileNumber, string? Otp) {
            var response = await this.AccountBL.ValidateOtpAsync(MobileNumber, Otp);
            
            if (response.StatusCode >= 400 && response.StatusCode < 500) return BadRequest(response.Messege);
            if (response.StatusCode >= 500 && response.StatusCode < 600)
                return StatusCode(StatusCodes.Status500InternalServerError, response.Messege);

            var cookieOptions = new CookieOptions
            {
                Expires = DateTimeOffset.Now.AddDays(7), // Cookie expires in 7 days
                HttpOnly = true, // Makes the cookie inaccessible to client-side java script
                Secure = true, // Only send the cookie over HTTPS
                SameSite = SameSiteMode.Lax // Controls when cookies are sent with cross-site requests
            };

            Response.Cookies.Append("UserRefreshToken", response.Data.RefreshToken, cookieOptions);

            return Ok(response);
        }


        [HttpGet("Refresh")]
        public async Task<IActionResult> RefreshAndCreateAccessToken() {
            HttpContext.Request.Cookies.TryGetValue("UserRefreshToken", out this.RefreshToken);
            var response = await this.AccountBL.RefreshAndCreateAccessToken(this.RefreshToken);

            if (response.StatusCode >= 400 && response.StatusCode < 500) return BadRequest(response.Message);
            if (response.StatusCode >= 500 && response.StatusCode < 600)
                return StatusCode(StatusCodes.Status500InternalServerError, response.Message);

            return Ok(response);
        }

        [Authorize]
        [HttpGet("UserOnly")]
        public string UserOnly() {
            return $"only user access end point";
        }

        [Authorize]
        [HttpPost("UpdateUserDetails")]
        public async Task<IActionResult> UpdateUserDetails(UserRequestDto UserRequestDto) {
            var response = await AccountBL.UpdateUserDetailsAsync(UserRequestDto);

            if (response.StatusCode >= 400 && response.StatusCode < 500) return BadRequest(response.Messege);
            if (response.StatusCode >= 500 && response.StatusCode < 600)
                return StatusCode(StatusCodes.Status500InternalServerError, response.Messege);

            return Ok(response);
        }

    }
}
