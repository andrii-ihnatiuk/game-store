using Northwind.Data.Entities;
using Northwind.Data.Interfaces;

namespace Northwind.Data;

public class MongoUnitOfWork : IMongoUnitOfWork
{
    private readonly IMongoContext _context;

    public MongoUnitOfWork(
        IMongoContext context,
        IGenericRepository<Order> ordersRepository,
        IOrderDetailRepository orderDetailRepository,
        IGenericRepository<Product> productsRepository,
        ICategoryRepository categoriesRepository,
        ISupplierRepository suppliersRepository)
    {
        _context = context;
        Orders = ordersRepository;
        OrderDetails = orderDetailRepository;
        Products = productsRepository;
        Categories = categoriesRepository;
        Suppliers = suppliersRepository;
    }

    public IGenericRepository<Order> Orders { get; }

    public IOrderDetailRepository OrderDetails { get; }

    public IGenericRepository<Product> Products { get; }

    public ICategoryRepository Categories { get; }

    public ISupplierRepository Suppliers { get; }

    public Task<bool> SaveChangesAsync()
    {
        return _context.SaveChangesAsync();
    }
}