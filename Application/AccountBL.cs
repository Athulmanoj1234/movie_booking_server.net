using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using Microsoft.EntityFrameworkCore;
using movie_booking.data;
using movie_booking.Dtos.Request;
using movie_booking.Dtos.Response;
using movie_booking.Models;
using movie_booking.services;
using System.Security.Cryptography;


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
        public Guid AdminId;
        public byte[] Salt;
        public string hashedAdminPassWord;
        public bool isLoggined;
        private string EnteredPassword;
        public string AdminAccessToken;
        public string AdminRefreshToken;
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

        public async Task<SuccessOrErrorResponseDto<Admin>> AdminRegister(AdminInfoRequestDto Admin) {

            if (string.IsNullOrEmpty(Admin.AdminEmail) || string.IsNullOrEmpty(Admin.AdminPassword)) {
                return new SuccessOrErrorResponseDto<Admin>()
                {
                    StatusCode = 400,
                    IsSuccess = false,
                    Messege = $"invalid or admin email/password is null or empty"
                };
            }

            try
            {
                var admin = await DbContext.Admins.FirstOrDefaultAsync(a => a.AdminEmail == Admin.AdminEmail);

                if (admin is null || string.IsNullOrEmpty(admin.AdminEmail) || string.IsNullOrEmpty(admin.AdminPassword)) {
                    this.AdminId = Guid.NewGuid();

                    this.Salt = RandomNumberGenerator.GetBytes(128 / 8);
                    //this.hashedAdminPassWord = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    //    password: Admin.AdminEmail!,
                    //    salt: this.Salt,
                    //    iterationCount: 1000,
                    //    prf: KeyDerivationPrf.HMACSHA256,  //prf tells Pbkdf2 that which hashing function needs to be used here we used for genrate hash
                    //    numBytesRequested: 256
                    //    ));
                    this.hashedAdminPassWord = new PasswordHasher<Admin>().HashPassword(null, Admin.AdminPassword);

                    var AdminEntity = new Admin()
                    {
                        AdminId = this.AdminId,
                        AdminEmail = Admin.AdminEmail,
                        AdminPassword = this.hashedAdminPassWord,
                        Role = "Admin",
                    };

                    this.DbContext.Add(AdminEntity);
                    this.DbContext.SaveChanges();
                    return new SuccessOrErrorResponseDto<Admin>() { 
                        StatusCode = 200,
                        IsSuccess = true,
                        Messege = $"successfully registered Admin Info",
                    };
                }

                return new SuccessOrErrorResponseDto<Admin>()
                {
                    StatusCode = 400,
                    IsSuccess = false,
                    Messege = $"Admin info already exists",
                };
            }
            catch (Exception ex) {
                return new SuccessOrErrorResponseDto<Admin>()
                {
                    StatusCode = 500,
                    IsSuccess = false,
                    Messege = ex.Message,
                };
            }
        }

        public async Task<SuccessOrErrorResponseDto<LoginResponseDto>> AdminLogin(AdminInfoRequestDto Admin) {

            this.EnteredPassword = Admin.AdminPassword;
            if (string.IsNullOrEmpty(Admin.AdminEmail) || string.IsNullOrEmpty(this.EnteredPassword))
            {
                return new SuccessOrErrorResponseDto<LoginResponseDto>()
                {
                    StatusCode = 400,
                    IsSuccess = false,
                    Messege = $"invalid or admin email/password is null or empty"
                };
            }

            try
            {
                var admin = await this.DbContext.Admins.FirstOrDefaultAsync(a => a.AdminEmail == Admin.AdminEmail);

                if (admin is null || string.IsNullOrEmpty(admin.AdminEmail) || string.IsNullOrEmpty(this.EnteredPassword)) {
                    return new SuccessOrErrorResponseDto<LoginResponseDto>()
                    {
                        StatusCode = 400,
                        IsSuccess = false,
                        Messege = $"admin info is not present in the db/failed to login",
                    };
                }
                if (new PasswordHasher<Admin>().VerifyHashedPassword(admin, admin.AdminPassword, this.EnteredPassword) == PasswordVerificationResult.Failed) {
                    return new SuccessOrErrorResponseDto<LoginResponseDto>()
                    {
                        StatusCode = 400,
                        IsSuccess = false,
                        Messege = $"failed to loggged/password entered is wrong",
                    };
                }

                this.AdminAccessToken = this.JwtService.GenerateAdminAccessToken(admin);
                this.AdminRefreshToken = await this.JwtService.GenerateAndStoreAdminRefreshTokenAsync(admin);

                return new SuccessOrErrorResponseDto<LoginResponseDto>()
                {
                    StatusCode = 200,
                    IsSuccess = true,
                    Messege = $"user successfully logged in",
                    Data = new LoginResponseDto() { 
                        AdminAccessToken = this.AdminAccessToken,
                        AdminRefreshToken = this.AdminRefreshToken,
                    },
                };
            }
            catch (Exception ex) {
                return new SuccessOrErrorResponseDto<LoginResponseDto>()
                {
                    StatusCode = 500,
                    IsSuccess = false,
                    Messege = ex.Message,
                };
            }
        }

        public async Task<RefreshResponseDto> RefreshAndCreateAdminAccessToken(string AdminRefreshToken)
        {
            if (string.IsNullOrEmpty(AdminRefreshToken))
            {
                return new RefreshResponseDto()
                {
                    StatusCode = 400,
                    IsSuccess = false,
                    Message = $"failed to send refresh token in cookies/cookies is null/empty in request",
                };
            }
            try
            {
                var admin = await this.DbContext.Admins.FirstOrDefaultAsync(a => a.AdminRefreshToken == AdminRefreshToken);
                bool IsAdminRefreshtokenValid = JwtService.ValidateAdminRefreshToken(admin, AdminRefreshToken);

                if (!IsAdminRefreshtokenValid)
                {
                    return new RefreshResponseDto()
                    {
                        StatusCode = 404,
                        IsSuccess = false,
                        Message = $"refresh token is invalid null or expired"
                    };
                }
                this.AdminAccessToken = this.JwtService.GenerateAdminAccessToken(admin);

                if (string.IsNullOrEmpty(this.AdminAccessToken))
                {
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
                    AccessToken = this.AdminAccessToken,
                };
            }

            catch (Exception ex)
            {
                return new RefreshResponseDto()
                {
                    StatusCode = 500,
                    IsSuccess = false,
                    Message = $"failed to create access token - internal server error",
                };
            }
        }
    }
    }
