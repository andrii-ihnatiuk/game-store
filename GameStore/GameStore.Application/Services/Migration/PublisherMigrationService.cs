using AutoMapper;
using GameStore.Application.Interfaces.Migration;
using GameStore.Data.Entities;
using GameStore.Data.Extensions;
using GameStore.Data.Interfaces;
using GameStore.Shared.DTOs.Publisher;
using Northwind.Data.Extensions;
using Northwind.Data.Interfaces;

namespace GameStore.Application.Services.Migration;

public class PublisherMigrationService : MigrationServiceBase, IPublisherMigrationService
{
    private readonly IUnitOfWork _coreUnitOfWork;
    private readonly IMongoUnitOfWork _mongoUnitOfWork;
    private readonly IMapper _mapper;

    public PublisherMigrationService(IUnitOfWork coreUnitOfWork, IMongoUnitOfWork mongoUnitOfWork, IMapper mapper)
    : base(coreUnitOfWork)
    {
        _coreUnitOfWork = coreUnitOfWork;
        _mongoUnitOfWork = mongoUnitOfWork;
        _mapper = mapper;
    }

    public async Task<PublisherUpdateDto> MigrateOnUpdateAsync(PublisherUpdateDto entity, bool commitMigration = true)
    {
        var publisher = entity.Publisher;
        if (IsEntityMigrationRequired(publisher.Id))
        {
            await Task.WhenAll(
                _mongoUnitOfWork.Suppliers.ThrowIfDoesNotExistWithId(publisher.Id),
                _coreUnitOfWork.Publishers.ThrowIfCompanyNameIsNotUnique(publisher.CompanyName));

            string legacyId = publisher.Id;
            publisher.Id = Guid.Empty.ToString();
            var migratedPublisher = _mapper.Map<Publisher>(entity);
            migratedPublisher.LegacyId = legacyId;

            await _coreUnitOfWork.Publishers.AddAsync(migratedPublisher);
            publisher.Id = migratedPublisher.Id.ToString();
        }

        await FinishMigrationAsync(commitMigration);
        return entity;
    }
}