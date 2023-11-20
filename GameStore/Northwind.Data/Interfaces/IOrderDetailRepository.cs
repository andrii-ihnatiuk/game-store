using Northwind.Data.Entities;

namespace Northwind.Data.Interfaces;

public interface IOrderDetailRepository : IGenericRepository<OrderDetail>
{
    Task<IList<OrderDetail>> GetAllByOrderObjectId(string id);
}