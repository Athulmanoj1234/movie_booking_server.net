using Amazon.Runtime;
using Amazon.S3;
using CommonServicesLibrary.data;
using CommonServicesLibrary.services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace CommonServicesLibrary
{
    public static class DependencyInjectionExtension
    {
        public static IServiceCollection AddServicesInLibrary(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IAmazonS3>(sp =>
            {
                var accessKey = configuration["R2:AccessKeyId"];
                var secretKey = configuration["R2:SecretAccessKey"];
                var accountId = configuration["R2:AccountId"];

                var credentials = new BasicAWSCredentials(accessKey, secretKey);
                var clientConfig = new AmazonS3Config
                {
                    ServiceURL = $"https://{accountId}.r2.cloudflarestorage.com",
                    ForcePathStyle = true // Required for R2 path-style routing ie the 
                                          //without configuring path style -
                                          //https://
                                          //     movie - files.
                                          //     123456.
                                          //     r2.cloudflarestorage.com /
                                          //     poster(example file name).jpg
                                          //with path style -
                                          //     https://
                                          //      123456.r2.cloudflarestorage.com /
                                          //      movie - files /
                                          //      poster.jpg
                };

                return new AmazonS3Client(credentials, clientConfig);
            });
            services.AddDbContextFactory<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            services.AddScoped<R2StorageService>();
            services.AddScoped<ClamAvScanner>();

            return services;
        }


        public static IServiceCollection AddServicesInLibraryForWorkerServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IAmazonS3>(sp =>
            {
                var accessKey = configuration["R2:AccessKeyId"];
                var secretKey = configuration["R2:SecretAccessKey"];
                var accountId = configuration["R2:AccountId"];

                var credentials = new BasicAWSCredentials(accessKey, secretKey);
                var clientConfig = new AmazonS3Config
                {
                    ServiceURL = $"https://{accountId}.r2.cloudflarestorage.com",
                    ForcePathStyle = true // Required for R2 path-style routing ie the 
                                          //without configuring path style -
                                          //https://
                                          //     movie - files.
                                          //     123456.
                                          //     r2.cloudflarestorage.com /
                                          //     poster(example file name).jpg
                                          //with path style -
                                          //     https://
                                          //      123456.r2.cloudflarestorage.com /
                                          //      movie - files /
                                          //      poster.jpg
                };

                return new AmazonS3Client(credentials, clientConfig);
            });
            services.AddDbContextFactory<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            return services;
        }
    }
}
