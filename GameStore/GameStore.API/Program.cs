using GameStore.API.Authorization;
using GameStore.API.Middlewares;
using GameStore.API.OptionsSetup;
using GameStore.Application.Configuration;
using GameStore.Data;
using GameStore.Data.Extensions;
using GameStore.Shared.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Azure;
using NLog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "angular-front",
        policyBuilder =>
        {
            policyBuilder.WithOrigins("http://127.0.0.1:8080", "http://localhost:8080")
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

builder.Services.Configure<RouteOptions>(options => { options.LowercaseUrls = true; });
builder.Services.Configure<AzureStorageOptions>(builder.Configuration.GetSection("Azure:Storage"));
builder.Services.Configure<VisaOptions>(builder.Configuration.GetSection("PaymentOptions:Visa"));
builder.Services.Configure<TerminalOptions>(builder.Configuration.GetSection("PaymentOptions:IBox"));
builder.Services.Configure<TaxOptions>(builder.Configuration.GetSection("PaymentOptions:Taxes"));
builder.Services.Configure<MongoDbOptions>(builder.Configuration.GetSection("MongoDbOptions"));
builder.Services.Configure<AuthApiOptions>(builder.Configuration.GetSection("AuthApiOptions"));
builder.Services.Configure<IdentityOptions>(builder.Configuration.GetSection("IdentityOptions"));
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("JwtOptions"));
builder.Services.ConfigureOptions<JwtBearerOptionsSetup>();
builder.Services.ConfigureOptions<RequestLocalizationOptionsSetup>();

builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer();
builder.Services.ConfigureAuthorization();

builder.Services.AddAzureClients(azureFactory =>
{
    azureFactory.AddBlobServiceClient(builder.Configuration["Azure:Storage:ConnectionString"]);
});

var app = builder.Build();

app.ApplyDatabaseMigrations();
IdentityDataInitializer.Initialize(app.Services);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRequestLocalization();
app.UseMiddleware<PerformanceLoggingMiddleware>();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<IpLoggingMiddleware>();
app.UseMiddleware<HeaderAdderMiddleware>();

app.UseHttpsRedirection();

app.UseCors("angular-front");

app.UseResponseCaching();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();