using GameStore.API.Middlewares;
using GameStore.Application.Configuration;
using GameStore.Shared.Settings;
using Microsoft.AspNetCore.Mvc;
using NLog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "angular-front",
        policyBuilder =>
        {
            policyBuilder.WithOrigins("http://127.0.0.1:8080")
                .WithExposedHeaders("x-total-numbers-of-games");
            policyBuilder.AllowAnyHeader();
            policyBuilder.AllowAnyMethod();
        });
});

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
builder.Services.AddMemoryCache();
builder.Services.AddHttpClient();

LogManager.Setup().LoadConfigurationFromFile("nlog.config", false);

builder.Services.Configure<RouteOptions>(options =>
{
    options.LowercaseUrls = true;
});
builder.Services.Configure<VisaSettings>(builder.Configuration.GetSection("Payment:VisaSettings"));
builder.Services.Configure<TerminalSettings>(builder.Configuration.GetSection("Payment:IBoxSettings"));
builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection(nameof(MongoDbSettings)));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<PerformanceLoggingMiddleware>();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<IpLoggingMiddleware>();
app.UseMiddleware<HeaderAdderMiddleware>();

app.UseHttpsRedirection();

app.UseCors("angular-front");

app.UseResponseCaching();

app.UseAuthorization();

app.MapControllers();

app.Run();
