using GameStore.API.Middlewares;
using GameStore.Services.Configuration;
using Microsoft.AspNetCore.Mvc;
using NLog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(opts =>
{
    opts.CacheProfiles.Add(
        "OneMinuteCache",
        new CacheProfile()
        {
            Duration = 60,
            Location = ResponseCacheLocation.Any,
            NoStore = builder.Environment.IsDevelopment(), // no store cache in development environment
        });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddResponseCaching();

LogManager.Setup().LoadConfigurationFromFile("nlog.config", false);

builder.Services.Configure<RouteOptions>(options =>
{
    options.LowercaseUrls = true;
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<IpLoggingMiddleware>();
app.UseMiddleware<PerformanceLoggingMiddleware>();

app.UseHttpsRedirection();

app.UseResponseCaching();

app.UseAuthorization();

app.MapControllers();

app.Run();
