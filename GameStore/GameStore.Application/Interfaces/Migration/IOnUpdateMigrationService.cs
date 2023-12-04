namespace GameStore.Application.Interfaces.Migration;

public interface IOnUpdateMigrationService<TEntity>
    where TEntity : class
{
    Task<TEntity> MigrateOnUpdateAsync(TEntity entity);
}