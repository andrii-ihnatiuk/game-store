using System.Diagnostics.CodeAnalysis;
using GameStore.Data.Interfaces;
using GameStore.Shared.Extensions;

namespace GameStore.Application.Services.Migration;

[ExcludeFromCodeCoverage]
public abstract class MigrationServiceBase
{
    private readonly IUnitOfWork _unitOfWork;

    protected MigrationServiceBase(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    protected static bool IsEntityMigrationNotRequired(string? entityId)
    {
        return string.IsNullOrEmpty(entityId) || entityId.IsGuidFormat();
    }

    protected static bool IsEntityMigrationRequired(string? entityId)
    {
        return !IsEntityMigrationNotRequired(entityId);
    }

    protected Task FinishMigrationAsync(bool commitMigration)
    {
        return commitMigration ? _unitOfWork.SaveAsync() : Task.CompletedTask;
    }
}