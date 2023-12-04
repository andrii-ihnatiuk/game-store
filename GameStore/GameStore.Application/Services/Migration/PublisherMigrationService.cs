using GameStore.Application.Interfaces.Migration;
using GameStore.Data.Entities;
using GameStore.Data.Extensions;
using GameStore.Data.Interfaces;
using GameStore.Shared.DTOs.Publisher;
using GameStore.Shared.Extensions;
using Northwind.Data.Extensions;
using Northwind.Data.Interfaces;

namespace GameStore.Application.Services.Migration;

public class PublisherMigrationService : IPublisherMigrationService
{
    private readonly IUnitOfWork _coreUnitOfWork;
    private readonly IMongoUnitOfWork _mongoUnitOfWork;

    public PublisherMigrationService(IUnitOfWork coreUnitOfWork, IMongoUnitOfWork mongoUnitOfWork)
    {
        _coreUnitOfWork = coreUnitOfWork;
        _mongoUnitOfWork = mongoUnitOfWork;
    }

    public async Task<PublisherUpdateDto> MigrateOnUpdateAsync(PublisherUpdateDto entity)
    {
        var publisher = entity.Publisher;
        if (publisher.Id.IsNotGuidFormat())
        {
            await Task.WhenAll(
                _mongoUnitOfWork.Suppliers.ThrowIfDoesNotExistWithId(publisher.Id),
                _coreUnitOfWork.Publishers.ThrowIfCompanyNameIsNotUnique(publisher.CompanyName));

            var migratedPublisher = new Publisher
            {
                LegacyId = publisher.Id,
                CompanyName = publisher.CompanyName,
                Description = publisher.Description,
                HomePage = publisher.HomePage,
            };
            await _coreUnitOfWork.Publishers.AddAsync(migratedPublisher);
            publisher.Id = migratedPublisher.Id.ToString();
        }

        await _coreUnitOfWork.SaveAsync();
        return entity;
    }
}