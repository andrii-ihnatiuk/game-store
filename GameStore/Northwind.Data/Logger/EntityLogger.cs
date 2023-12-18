using MongoDB.Bson;
using Newtonsoft.Json;
using Northwind.Data.Entities;
using Northwind.Data.Interfaces;

namespace Northwind.Data.Logger;

public class EntityLogger : IEntityLogger
{
    private const string CreateAction = "CREATE";
    private const string UpdateAction = "UPDATE";
    private const string DeleteAction = "DELETE";

    private readonly IMongoUnitOfWork _unitOfWork;

    public EntityLogger(IMongoUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task LogCreateAsync(object entity)
    {
        _unitOfWork.Logs.Add(GetEntityLog(CreateAction, entity));
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task LogUpdateAsync(object oldEntity, object updEntity)
    {
        _unitOfWork.Logs.Add(GetEntityLog(UpdateAction, oldEntity, updEntity));
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task LogDeleteAsync(object entity)
    {
        _unitOfWork.Logs.Add(GetEntityLog(DeleteAction, entity));
        await _unitOfWork.SaveChangesAsync();
    }

    private static EntityLog GetEntityLog(string action, object entity)
    {
        return new EntityLog
        {
            Action = action,
            Date = DateTime.UtcNow,
            EntityType = entity.GetType().ToString(),
            Entity = GetBsonDocument(entity),
        };
    }

    private static EntityLog GetEntityLog(string action, object oldEntity, object updEntity)
    {
        var log = GetEntityLog(action, oldEntity);
        log.UpdEntity = GetBsonDocument(updEntity);
        return log;
    }

    private static BsonDocument GetBsonDocument(object entity)
    {
        string json = JsonConvert.SerializeObject(entity, settings: new JsonSerializerSettings()
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
        });
        return BsonDocument.Parse(json);
    }
}