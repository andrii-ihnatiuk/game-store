namespace Northwind.Data.Interfaces;

public interface IEntityLogger
{
    Task LogCreateAsync(object entity);

    Task LogUpdateAsync(object oldEntity, object updEntity);

    Task LogDeleteAsync(object entity);
}