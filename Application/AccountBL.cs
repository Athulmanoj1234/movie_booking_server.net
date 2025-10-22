using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using movie_booking.data;
using movie_booking.Dtos.Request;
using movie_booking.Dtos.Response;
using movie_booking.Models;
using movie_booking.services;
using System.Runtime.CompilerServices;

namespace movie_booking.Application
{
    public class AccountBL : DelegatingHandler
    {
        private IConfiguration Congiguration;
        private ApplicationDbContext DbContext;
        public JwtService JwtService;
        public string MobileNumberPrefix;
        public string MobileNumberWithoutPrefix;
        public string MobileNumber;
        public string Otp;
        public string AccessToken;
        public string RefreshToken;
        public bool isLoogedIn = false;
        //public SuccuessOrErrorResponseDto SuccessResponse;

        public AccountBL(IConfiguration configuration, ApplicationDbContext dbContext, JwtService jwtService) {
            this.Congiguration = configuration;
            this.DbContext = dbContext;
        }

        public async Task<SuccessOrErrorResponseDto<User>> GetOrAddUserbyMobileAsync(string mobileNumber) {

            if (string.IsNullOrEmpty(mobileNumber)) {
                return new SuccessOrErrorResponseDto<User>()
                {
                    StatusCode = 400,
                    IsSuccess = false,
                    Messege = $"invalid or mobile number is null or empty"
                };
            }

            try
            {
                this.MobileNumberPrefix = mobileNumber.Split(' ')[0];
                this.MobileNumberWithoutPrefix = mobileNumber.Split(' ')[1];

                var user = await this.DbContext.Users
                    .FirstOrDefaultAsync(u => u.MobileNumber == MobileNumberWithoutPrefix);

                if (user is null || string.IsNullOrEmpty(user.MobileNumber)
                    || string.IsNullOrEmpty(user.FirstName) || string.IsNullOrEmpty(user.LastName)
                    || string.IsNullOrEmpty(user.Email)) {
                    
                    var userId = Guid.NewGuid();
                    var userEntity = new User()
                    {
                        UserId = userId,
                        MobileNumber = MobileNumberWithoutPrefix,
                        MobileNumberPrefix = MobileNumberPrefix
                    };
                    var userCreate = this.DbContext.Users.Add(userEntity);
                    this.DbContext.SaveChanges();
                    return new SuccessOrErrorResponseDto<User>()
                    {
                        StatusCode = 204,
                        IsSuccess = false,
                        Messege = $"the user does not exists with the number {mobileNumber} please register",
                    };
                }
                return new SuccessOrErrorResponseDto<User>()
                {
                    StatusCode = 200,
                    IsSuccess = true,
                    Messege = $"user with number {mobileNumber} exists",
                    Data = user,
                };
            }
            catch (Exception ex) {
                return new SuccessOrErrorResponseDto<User>()
                {
                    StatusCode = 500,
                    IsSuccess = false,
                    Messege = $"error in fetching user details",
                };
            }
        }

        public async Task<SuccessOrErrorResponseDto<LoginResponseDto>> ValidateOtpAsync(string MobileNumber, string? Otp) {
            if (string.IsNullOrEmpty(MobileNumber))
            {  // || string.IsNullOrEmpty(Otp)
                return new SuccessOrErrorResponseDto<LoginResponseDto>()
                {
                    StatusCode = 400,
                    IsSuccess = false,
                    Messege = $"mobile number or otp is empty or null"
                };
            }

            try {
                var user = await this.DbContext.Users.FirstOrDefaultAsync(u => u.MobileNumber == MobileNumber);
                if (user is null || user.UserId == Guid.Empty || string.IsNullOrEmpty(user.MobileNumberPrefix)
                    || string.IsNullOrEmpty(user.MobileNumber)) {
                    return new SuccessOrErrorResponseDto<LoginResponseDto>()
                    {
                        StatusCode = 204,
                        IsSuccess = false,
                        Messege = "user needs to be regitered/no user details in db",
                    };
                }
                this.isLoogedIn = true;

                if (this.isLoogedIn) {
                    var accessToken = this.JwtService.GenerateAccessToken(user);
                    var refreshToken = await this.JwtService.GenerateAndStoreRefreshTokenAsync(user);
                    var cookieOptions = new CookieOptions
                    {
                        Expires = DateTime.UtcNow.AddDays(4),
                        HttpOnly = true, //making cookes inaccessable via client side js
                        Secure = true, //only sent cookies over https
                        SameSite = SameSiteMode.None,
                    };
                    if (string.IsNullOrEmpty(accessToken) || string.IsNullOrEmpty(refreshToken)) {
                        return new SuccessOrErrorResponseDto<LoginResponseDto>() { 
                            StatusCode = 500,
                            IsSuccess = false,
                            Messege = $"failed to create accesstoken or refresh token",
                        };
                    }

                    return new SuccessOrErrorResponseDto<LoginResponseDto>()
                    {
                        StatusCode = 200,
                        IsSuccess = true,
                        Messege = $"successfully logined created token and refresh token",
                        Data = new LoginResponseDto() {
                            AccessToken = this.AccessToken,
                            RefreshToken = this.RefreshToken,
                        }
                    }; 
                }

                return new SuccessOrErrorResponseDto<LoginResponseDto>()
                {
                    IsSuccess = false,
                    Messege = $"incorrect credentials or failed to login with mobile number try correct mobile number"
                };
            }

            catch(Exception e) {
                return new SuccessOrErrorResponseDto<LoginResponseDto>() {
                    StatusCode = 500,
                    IsSuccess = false,
                    Messege = e.Message,
                };
            }
        }
    }
}
