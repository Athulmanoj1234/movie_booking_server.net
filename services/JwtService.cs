using Microsoft.IdentityModel.Tokens;
using movie_booking.data;
using movie_booking.Models;
using System.ComponentModel;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace movie_booking.services
{
    public class JwtService
    {
        public IConfiguration Configuration;
        private string _refreshToken;
        private DateTime _refreshTokenExpiry;
        private ApplicationDbContext _dbContext;
        public string _adminRefreshToken;
        public DateTime _adminRefreshTokenExpiry;

        public JwtService(IConfiguration Configuration, ApplicationDbContext DbContext) {
            this.Configuration = Configuration;
            this._dbContext = DbContext;
        }
        public string GenerateAccessToken(User user) {
            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Sub, Configuration["Jwt:UserSubject"]),
                new Claim(JwtRegisteredClaimNames.Jti, user.UserId.ToString()),
                new Claim("UserName", $"{user.FirstName} {user.LastName}"),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]));

            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            try
            {
                var token = new JwtSecurityToken(
                    issuer: Configuration["Jwt:Issuer"],
                    audience: Configuration["jwt:Audience"],
                    claims,
                    expires: DateTime.UtcNow.AddMinutes(10),
                    signingCredentials: signIn
                    );
                var accessToken = new JwtSecurityTokenHandler().WriteToken(token);
                return accessToken;

            } catch (Exception e) {
                return e.Message;
            }
        }

        public string GenerateRefreshToken() {
            var random = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(random);
            return Convert.ToBase64String(random);
        }

        public async Task<string> GenerateAndStoreRefreshTokenAsync(User user) {
            try
            {
                this._refreshToken = GenerateRefreshToken();
                this._refreshTokenExpiry = DateTime.UtcNow.AddDays(4);
                user.RefreshToken = this._refreshToken;
                user.RefreshTokenExpirytime = this._refreshTokenExpiry;
                await this._dbContext.SaveChangesAsync();
                return this._refreshToken;
            }
            catch (Exception e) {
                return e.Message;
            }
        }

        public bool ValidateRefreshToken(User user, string RefreshToken) {
            if (user is null || user.RefreshToken != RefreshToken || user.RefreshTokenExpirytime <= DateTime.UtcNow) {
                return false;
            }
            return true;
        }


        //admin

        public string GenerateAdminAccessToken(Admin Admin)
        {
            var claims = new [] {
                new Claim(JwtRegisteredClaimNames.Sub, Configuration["Jwt:AdminSubject"]),
                new Claim(JwtRegisteredClaimNames.Jti, Admin.AdminId.ToString()),
                new Claim("AdminEmail", Admin.AdminEmail),
                new Claim(ClaimTypes.Role, Admin.Role),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]));

            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            try
            {
                var token = new JwtSecurityToken(
                    issuer: Configuration["Jwt:Issuer"],
                    audience: Configuration["jwt:Audience"],
                    claims,
                    expires: DateTime.UtcNow.AddSeconds(10),
                    signingCredentials: signIn
                    );
                var accessToken = new JwtSecurityTokenHandler().WriteToken(token);
                return accessToken;

            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public string GenerateAdminRefreshToken()
        {
            var random = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(random);
            return Convert.ToBase64String(random);
        }

        public async Task<string> GenerateAndStoreAdminRefreshTokenAsync(Admin Admin)
        {
            try
            {
                this._adminRefreshToken = GenerateAdminRefreshToken();
                this._adminRefreshTokenExpiry = DateTime.UtcNow.AddDays(4);
                Admin.AdminRefreshToken = this._adminRefreshToken;
                Admin.AdminRefreshTokenExpiryTime = this._adminRefreshTokenExpiry;
                await this._dbContext.SaveChangesAsync();
                return this._adminRefreshToken;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public bool ValidateAdminRefreshToken(Admin admin, string AdminRefreshToken)
        {
            if (admin is null || admin.AdminRefreshToken != AdminRefreshToken || admin.AdminRefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                return false;
            }
            return true;
        }

    }
}
