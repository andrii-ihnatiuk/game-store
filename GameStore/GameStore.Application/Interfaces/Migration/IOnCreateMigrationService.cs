namespace GameStore.Application.Interfaces.Migration;

public interface IOnCreateMigrationService<TEntity>
    where TEntity : class
{
    Task<TEntity> MigrateOnCreateAsync(TEntity entity);
}