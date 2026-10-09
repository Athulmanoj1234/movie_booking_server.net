using Amazon.Runtime;
using Amazon.Runtime.Internal.Endpoints.StandardLibrary;
using Amazon.S3;
using Amazon.S3.Model;
using Azure.Core;
using CommonServicesLibrary;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Microsoft.VisualBasic;
using movie_booking.Application;
using movie_booking.Controllers;
using movie_booking.data;
using movie_booking.services;
using movie_booking.services.RateLimitingServices;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.RateLimiting;
using static System.Net.WebRequestMethods;


var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";  //cors policy name
// Add services to the container.
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<AccountBL>();
builder.Services.AddScoped<MovieDetailBL>();
builder.Services.AddScoped<TheatreBL>();
builder.Services.AddScoped<JwtService>();
builder.Services.AddScoped<PasswordHashService>();
builder.Services.AddScoped<MovieDetailsService>();
builder.Services.AddScoped<FileUploadService>();
builder.Services.AddScoped<R2StorageService>();
builder.Services.AddScoped<ClamAvScanner>();

//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//builder.Services.AddDbContextFactory<ApplicationDbContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
//);

builder.Services.AddServicesInLibrary(builder.Configuration);

builder.Services.AddCors(options =>
    options.AddPolicy(name: MyAllowSpecificOrigins,
    policy =>
    {
        policy.WithOrigins("http://localhost:3000", "http://localhost:4200")
              .AllowCredentials()
              .AllowAnyHeader() //allows any custom headers to be sent with request in frontend
              .AllowAnyMethod(); //allow any http methods 
    })
); //reisters the cors policy named MyAllowSpecificOrigins

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:key"]))
    };
});


// create ip based partitioned ratelimiter in sliding window algorithm
var _ipLimiter = PartitionedRateLimiter.Create<string, string>(ip =>
                   // get the partition ratelimiter based on the partition key in here it is ip
                   // await _ipLimiter.AcquireAsync("192.168.1.10", 1); when this code is called ie when the request needs to check the acquisition the RateLimitPartition.Get() is called 
                   //However, RateLimitPartition.Get itself is not the method that checks whether a request is allowed.It supplies the partition information and the factory that can create the limiter for that partition.The actual permit acquisition happens when you call AcquireAsync or another acquisition API.
                   RateLimitPartition.Get(ip,
   _ => new SlidingWindowRateLimiter(new SlidingWindowRateLimiterOptions
   {
       PermitLimit = 5,
       Window = TimeSpan.FromMinutes(1),
       SegmentsPerWindow = 2,
       QueueLimit = 0,
       AutoReplenishment = true
   }
        )
    ));

var _userLimiter = PartitionedRateLimiter.Create<string, string>(adminId =>
                   // creating partition key based on the user details from the token
                   RateLimitPartition.Get(adminId,
   _ => new SlidingWindowRateLimiter(new SlidingWindowRateLimiterOptions
   {
       PermitLimit = 10,
       Window = TimeSpan.FromMinutes(1),
       SegmentsPerWindow = 2,
       QueueLimit = 0,
       AutoReplenishment = true
   }
        )
    ));

// implementing sliding window ratelimiter for login endpoint
builder.Services.AddRateLimiter(options =>
{
    //rateLimiterOptions.AddSlidingWindowLimiter("sliding", options =>
    //{
    //    options.PermitLimit = 5; // how much requests in one window timespan
    //    options.Window = TimeSpan.FromSeconds(10);
    //    options.SegmentsPerWindow = 2; // to how many segments should window should get break down
    //    options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst; // how the rejected requests comes ie the requests that were in the queue in the oldest that will process first.
    //    options.QueueLimit = 5; // upto how much rejected requests will stored in queue
    //});

    // policy based because it needs to add separately to an endpoint ie specifically to an endpoint
    options.AddPolicy<string>("ip-and-user", httpContext =>
    {
        var ip =
            httpContext.Connection.RemoteIpAddress?.ToString()
            ?? "unknown";

        var adminId = httpContext.User
            .FindFirst(JwtRegisteredClaimNames.Jti)?.Value ?? "anonymous";

        // Create coordinator for this request's
        // IP + user.
        //
        // IMPORTANT:
        // The actual IP/user limiter state is NOT
        // stored inside this coordinator.
        //
        // The state is stored inside the two
        // shared PartitionedRateLimiter objects above.
        return new IpUserRateLimiting(
            ip,
            adminId,
            _ipLimiter,
            _userLimiter);

    });

    // when the request more than the permit count then rejection response needs to be handled 

    options.OnRejected = async (
        context,
        cancellationToken) =>
    {
        context.HttpContext.Response.StatusCode =
            StatusCodes.Status429TooManyRequests;

        await context.HttpContext.Response.WriteAsync(
            "Too many requests.",
            cancellationToken);
    };

});

//builder.Services.AddSingleton<IAmazonS3>(sp =>
//{
//    var configuration = sp.GetRequiredService<IConfiguration>();
//    var accessKey = configuration["R2:AccessKeyId"];
//    var secretKey = configuration["R2:SecretAccessKey"];
//    var accountId = configuration["R2:AccountId"];

//    var credentials = new BasicAWSCredentials(accessKey, secretKey);
//    var clientConfig = new AmazonS3Config
//    {
//        ServiceURL = $"https://{accountId}.r2.cloudflarestorage.com",
//        ForcePathStyle = true // Required for R2 path-style routing ie the 
//        //without configuring path style -
//        //https://
//        //     movie - files.
//        //     123456.
//        //     r2.cloudflarestorage.com /
//        //     poster(example file name).jpg
//        //with path style -
//        //     https://
//        //      123456.r2.cloudflarestorage.com /
//        //      movie - files /
//        //      poster.jpg
//    };

//    return new AmazonS3Client(credentials, clientConfig);
//});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(MyAllowSpecificOrigins);  //Adds the CORS middleware to the HTTP request pipeline.
//It checks every incoming request’s Origin header (the domain the request came from).

app.UseAuthentication(); //If the token is valid, this middleware it creates a ClaimsPrincipal (the user).
app.UseAuthorization();

app.MapControllers();

app.Run();
