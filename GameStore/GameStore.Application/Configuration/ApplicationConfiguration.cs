﻿using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using FluentValidation;
using GameStore.Application.Interfaces;
using GameStore.Application.Interfaces.Migration;
using GameStore.Application.Interfaces.Util;
using GameStore.Application.Services;
using GameStore.Application.Services.Migration;
using GameStore.Application.Util;
using GameStore.Data.Configuration;
using GameStore.Services.Configuration;
using GameStore.Shared.DTOs.Comment;
using GameStore.Shared.DTOs.Game;
using GameStore.Shared.DTOs.Genre;
using GameStore.Shared.DTOs.Payment;
using GameStore.Shared.DTOs.Platform;
using GameStore.Shared.DTOs.Publisher;
using GameStore.Shared.DTOs.User;
using GameStore.Shared.Loggers;
using GameStore.Shared.Validators;
using GameStore.Shared.Validators.CommentValidators;
using GameStore.Shared.Validators.GameValidators;
using GameStore.Shared.Validators.GenreValidators;
using GameStore.Shared.Validators.PaymentValidators;
using GameStore.Shared.Validators.PlatformValidators;
using GameStore.Shared.Validators.PublisherValidators;
using GameStore.Shared.Validators.UserValidators;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Northwind.Data.Configuration;
using Northwind.Services.Configuration;

namespace GameStore.Application.Configuration;

[ExcludeFromCodeCoverage]
public static class ApplicationConfiguration
{
    public static void AddApplicationServices(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddAutoMapper(
            typeof(GameStore.Services.MappingProfiles.GameProfile).Assembly,
            typeof(Northwind.Services.MappingProfiles.OrderProfile).Assembly);

        serviceCollection.AddSingleton<ILogger, NLogLogger>();

        serviceCollection.AddScoped<IOrderFacadeService, OrderFacadeService>();
        serviceCollection.AddScoped<IGameFacadeService, GameFacadeService>();
        serviceCollection.AddScoped<IGenreFacadeService, GenreFacadeService>();
        serviceCollection.AddScoped<IPublisherFacadeService, PublisherFacadeService>();

        serviceCollection.AddScoped<IEntityServiceResolver, EntityServiceResolver>();
        serviceCollection.AddScoped<ILoginServiceResolver, LoginServiceResolver>();
        serviceCollection.AddScoped<IServiceProviderWrapper, ServiceProviderWrapper>();

        serviceCollection.AddScoped<IGenreMigrationService, GenreMigrationService>();
        serviceCollection.AddScoped<IPublisherMigrationService, PublisherMigrationService>();
        serviceCollection.AddScoped<IGameMigrationService, GameMigrationService>();

        serviceCollection.AddScoped<IValidator<GameCreateDto>, GameCreateValidator>();
        serviceCollection.AddScoped<IValidator<GameUpdateDto>, GameUpdateValidator>();
        serviceCollection.AddScoped<IValidator<PublisherCreateDto>, PublisherCreateValidator>();
        serviceCollection.AddScoped<IValidator<PublisherUpdateDto>, PublisherUpdateValidator>();
        serviceCollection.AddScoped<IValidator<PlatformCreateDto>, PlatformCreateValidator>();
        serviceCollection.AddScoped<IValidator<PlatformUpdateDto>, PlatformUpdateValidator>();
        serviceCollection.AddScoped<IValidator<GenreCreateDto>, GenreCreateValidator>();
        serviceCollection.AddScoped<IValidator<GenreUpdateDto>, GenreUpdateValidator>();
        serviceCollection.AddScoped<IValidator<CommentCreateDto>, CommentCreateValidator>();
        serviceCollection.AddScoped<IValidator<CommentUpdateDto>, CommentUpdateValidator>();
        serviceCollection.AddScoped<IValidator<PaymentDto>, PaymentValidator>();
        serviceCollection.AddScoped<IValidator<GamesFilterDto>, GamesFilterValidator>();
        serviceCollection.AddScoped<IValidator<ContactInfoUpdateDto>, ContactInfoUpdateValidator>();
        serviceCollection.AddScoped(typeof(IValidatorWrapper<>), typeof(ValidatorWrapper<>));
        ValidatorOptions.Global.LanguageManager.Culture = new CultureInfo("en");

        serviceCollection.AddGameStoreDataAccess(configuration);
        serviceCollection.AddNorthwindDataAccess();
        serviceCollection.AddCoreServices();
        serviceCollection.AddNorthwindServices();
    }
}