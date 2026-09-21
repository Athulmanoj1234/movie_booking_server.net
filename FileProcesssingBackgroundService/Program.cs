using FileProcesssingBackgroundService;
using CommonServicesLibrary;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

builder.Services.AddServicesInLibrary(builder.Configuration);

var host = builder.Build();
host.Run();
