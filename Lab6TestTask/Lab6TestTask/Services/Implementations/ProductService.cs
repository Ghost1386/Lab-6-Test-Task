using Lab6TestTask.Data;
using Lab6TestTask.Models;
using Lab6TestTask.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Lab6TestTask.Services.Implementations;

/// <summary>
/// ProductService.
/// Implement methods here.
/// </summary>
public class ProductService : IProductService
{
    private readonly ApplicationDbContext _dbContext;
    private const int ReceivedDateYear = 2025;
    private const int ProductQuantity = 1000;

    public ProductService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Product> GetProductAsync()
    {
        var product = await _dbContext.Products
            .Where(p => p.ReceivedDate < DateTime.Now)
            .OrderBy(p => p.Price)
            .FirstAsync();

        return product; 
    }

    public async Task<IEnumerable<Product>> GetProductsAsync()
    {
        var products = await _dbContext.Products
            .Where(p => p.ReceivedDate.Year == ReceivedDateYear && 
                        p.Quantity > ProductQuantity)
            .ToListAsync();

        return products;
    }
}
