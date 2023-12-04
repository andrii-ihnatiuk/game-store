namespace GameStore.Application.Interfaces.Migration;

public interface IEntityMigrationService<TUpdate, TCreate>
    where TUpdate : class
    where TCreate : class
{
    Task<TUpdate> MigrateOnUpdateAsync(TUpdate entity);

    Task<TCreate> MigrateOnCreateAsync(TCreate entity);
}