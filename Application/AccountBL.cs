using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using movie_booking.data;
using movie_booking.Dtos.Request;
using movie_booking.Dtos.Response;
using movie_booking.Models;
using movie_booking.services;


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
        public bool isLoggedIn = false;
        //public SuccuessOrErrorResponseDto SuccessResponse;

        public AccountBL(IConfiguration configuration, ApplicationDbContext dbContext, JwtService jwtService)
        {
            this.Congiguration = configuration;
            this.DbContext = dbContext;
            this.JwtService = jwtService;
        }

        public async Task<SuccessOrErrorResponseDto<User>> GetOrAddUserbyMobileAsync(string mobileNumber)
        {

            if (string.IsNullOrEmpty(mobileNumber))
            {
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
                    || string.IsNullOrEmpty(user.Email))
                {

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
            catch (Exception ex)
            {
                return new SuccessOrErrorResponseDto<User>()
                {
                    StatusCode = 500,
                    IsSuccess = false,
                    Messege = $"error in fetching user details",
                };
            }
        }

        public async Task<SuccessOrErrorResponseDto<LoginResponseDto>> ValidateOtpAsync(string MobileNumber, string? Otp)
        {
            if (string.IsNullOrEmpty(MobileNumber))
            {  // || string.IsNullOrEmpty(Otp)
                return new SuccessOrErrorResponseDto<LoginResponseDto>()
                {
                    StatusCode = 400,
                    IsSuccess = false,
                    Messege = $"mobile number or otp is empty or null"
                };
            }

            try
            {
                var user = await this.DbContext.Users.FirstOrDefaultAsync(u => u.MobileNumber == MobileNumber);
                if (user is null || user.UserId == Guid.Empty || string.IsNullOrEmpty(user.MobileNumberPrefix)
                    || string.IsNullOrEmpty(user.MobileNumber))
                {
                    return new SuccessOrErrorResponseDto<LoginResponseDto>()
                    {
                        StatusCode = 204,
                        IsSuccess = false,
                        Messege = "user needs to be regitered/no user details in db",
                    };
                }
                this.isLoggedIn = true;
                //write otp logic here
                if (!this.isLoggedIn)
                {
                    return new SuccessOrErrorResponseDto<LoginResponseDto>()
                    {
                        StatusCode = 400,
                        IsSuccess = false,
                        Messege = $"incorrect credentials or failed to login with mobile number try correct mobile number"
                    };
                }
                this.AccessToken = this.JwtService.GenerateAccessToken(user);
                this.RefreshToken = await this.JwtService.GenerateAndStoreRefreshTokenAsync(user);

                if (string.IsNullOrEmpty(this.AccessToken) || string.IsNullOrEmpty(this.RefreshToken))
                {
                    return new SuccessOrErrorResponseDto<LoginResponseDto>()
                    {
                        StatusCode = 500,
                        IsSuccess = false,
                        Messege = $"failed to create accesstoken or refresh token",
                    };
                }

                return new SuccessOrErrorResponseDto<LoginResponseDto>()
                {
                    StatusCode = 200,
                    IsSuccess = true,
                    Messege = $"successfully logged in and created auth accesstoken and refreshtoken",
                    Data = new LoginResponseDto()
                    {
                        AccessToken = this.AccessToken,
                        RefreshToken = this.RefreshToken,
                    }
                };
            }

            catch (Exception e)
            {
                return new SuccessOrErrorResponseDto<LoginResponseDto>()
                {
                    StatusCode = 500,
                    IsSuccess = false,
                    Messege = e.Message,
                };
            }
        }


        public async Task<SuccessOrErrorResponseDto<UsersDto>> UpdateUserDetailsAsync(UserRequestDto UserRequestDto)
        {
            if (string.IsNullOrEmpty(UserRequestDto.FirstName) || string.IsNullOrEmpty(UserRequestDto.LastName)
                || string.IsNullOrEmpty(UserRequestDto.Email))
            {
                return new SuccessOrErrorResponseDto<UsersDto>()
                {
                    StatusCode = 400,
                    IsSuccess = false,
                    Messege = $"first name || last name || email is empty or null",
                };
            }

            try
            {
                var user = await DbContext.Users.FirstOrDefaultAsync(u => u.MobileNumber == UserRequestDto.MobileNumber);

                if (!string.IsNullOrEmpty(user.FirstName) || !string.IsNullOrEmpty(user.LastName) || !string.IsNullOrEmpty(user.Email)) {
                    return new SuccessOrErrorResponseDto<UsersDto>()
                    {
                        StatusCode = 400,
                        IsSuccess = false,
                        Messege = $"user is registered will full detiails so no need to edit the user details",
                    };
                }

                if (user == null || user.UserId == Guid.Empty || string.IsNullOrEmpty(user.MobileNumber) || string.IsNullOrEmpty(user.MobileNumberPrefix))
                {
                    return new SuccessOrErrorResponseDto<UsersDto>()
                    {
                        StatusCode = 404,
                        IsSuccess = false,
                        Messege = $"user info not available/not present in the database",
                    };
                }

                var userEntity = new User()
                {
                    FirstName = UserRequestDto.FirstName,
                    LastName = UserRequestDto.LastName,
                    Email = UserRequestDto.Email,
                };
                DbContext.Users.Add(userEntity);
                DbContext.SaveChanges();
                return new SuccessOrErrorResponseDto<UsersDto>() {
                    StatusCode = 200,
                    IsSuccess = true,
                    Messege = $"successfully added/updated user fields",
                };
             }
            catch (Exception ex) {
                return new SuccessOrErrorResponseDto<UsersDto>()
                {
                    StatusCode = 500,
                    IsSuccess = false,
                    Messege = ex.Message,
                };
            }
            }

        public async Task<RefreshResponseDto> RefreshAndCreateAccessToken(string RefreshToken) {
            if (string.IsNullOrEmpty(RefreshToken)) {
                return new RefreshResponseDto()
                {
                    StatusCode = 400,
                    IsSuccess = false,
                    Message = $"failed to send refresh token in cookies/cookies is null/empty in request",
                };
            }
            try {
                var user = await this.DbContext.Users.FirstOrDefaultAsync(u => u.RefreshToken == RefreshToken);
                bool IsRefreshtokenValid = JwtService.ValidateRefreshToken(user, RefreshToken);

                if (!IsRefreshtokenValid) {
                    return new RefreshResponseDto()
                    {
                        StatusCode = 404,
                        IsSuccess = false,
                        Message = $"refresh token is invalid null or expired"
                    };
                }
                this.AccessToken = this.JwtService.GenerateAccessToken(user);

                if (string.IsNullOrEmpty(this.AccessToken)) {
                    return new RefreshResponseDto()
                    {
                        StatusCode = 500,
                        IsSuccess = false,
                        Message = $"failed to create accesstoken"
                    };
                }

                return new RefreshResponseDto()
                {
                    StatusCode = 200,
                    IsSuccess = true,
                    Message = $"refreshed ad successfully created access token",
                    AccessToken = this.AccessToken,
                };
            }

            catch (Exception ex) {
                return new RefreshResponseDto()
                {
                    StatusCode = 500,
                    IsSuccess = false,
                    Message = $"failed to create access token - internal server error",
                    AccessToken = this.AccessToken,
                };
            }
        }
    }
    }
