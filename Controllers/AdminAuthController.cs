using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using movie_booking.Application;
using movie_booking.Dtos.Request;
using movie_booking.Models;

namespace movie_booking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminAuthController : ControllerBase
    {
        public AccountBL AccountBL;
        public string AdminRefreshToken;
        private HttpContext HttpContext;
        public AdminAuthController(AccountBL accountBL) { 
            this.AccountBL = accountBL;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> AdminRegister(AdminInfoRequestDto Admin) {
            var response = await this.AccountBL.AdminRegister(Admin);

            if (response.StatusCode >= 400 && response.StatusCode < 500) return BadRequest(response.Messege);
            if (response.StatusCode >= 500 && response.StatusCode < 600)
                return StatusCode(StatusCodes.Status500InternalServerError, response.Messege);

            return Ok(response);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> AdminLogin(AdminInfoRequestDto Admin) {
            var response = await this.AccountBL.AdminLogin(Admin);

            if (response.StatusCode >= 400 && response.StatusCode < 500) return BadRequest(response.Messege);
            if (response.StatusCode >= 500 && response.StatusCode < 600)
                return StatusCode(StatusCodes.Status500InternalServerError, response.Messege);

            var cookieOptions = new CookieOptions
            {
                Expires = DateTimeOffset.Now.AddDays(7), // Cookie expires in 7 days
                HttpOnly = true, // Makes the cookie inaccessible to client-side javascript
                Secure = true, // Only send the cookie over HTTPS
                SameSite = SameSiteMode.Lax // Controls when cookies are sent with cross-site requests
            };

            Response.Cookies.Append("AdminRefreshToken", response.Data.AdminRefreshToken, cookieOptions);

            return Ok(response);
        }

        [HttpGet("Refresh")]
        public async Task<IActionResult> RefreshAndCreateAdminAccessToken()
        {
            HttpContext.Request.Cookies.TryGetValue("AdminRefreshToken", out this.AdminRefreshToken);
            var response = await this.AccountBL.RefreshAndCreateAdminAccessToken(this.AdminRefreshToken);

            if (response.StatusCode >= 400 && response.StatusCode < 500) return BadRequest(response.Message);
            if (response.StatusCode >= 500 && response.StatusCode < 600)
                return StatusCode(StatusCodes.Status500InternalServerError, response.Message);

            return Ok(response);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> AdminLgout() {
            Request.Cookies.TryGetValue("AdminRefreshToken", out this.AdminRefreshToken);
            var response = await this.AccountBL.AdminLogout(this.AdminRefreshToken);


            if (response.StatusCode >= 400 && response.StatusCode < 500) return BadRequest(response.Messege);
            if (response.StatusCode >= 500 && response.StatusCode < 600)
                return StatusCode(StatusCodes.Status500InternalServerError, response.Messege);

            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("AdminOnly")]
        public string AdminOnly() {
            return $"this is only for admin only access";
        }
    }
}
