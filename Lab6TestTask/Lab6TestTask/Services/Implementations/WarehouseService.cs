using Lab6TestTask.Data;
using Lab6TestTask.Enums;
using Lab6TestTask.Models;
using Lab6TestTask.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Lab6TestTask.Services.Implementations;

/// <summary>
/// WarehouseService.
/// Implement methods here.
/// </summary>
public class WarehouseService : IWarehouseService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly DateTime StartReceivedDate = new DateTime(2025, 4, 1);
    private readonly DateTime EndReceivedDate = new DateTime(2025, 6, 30);

    public WarehouseService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Warehouse> GetWarehouseAsync()
    {
        var warehouse = await _dbContext.Warehouses
            .Select(w => new
            {
                Warehouse = w,
                TotalCost = w.Products
                    .Where(p => p.Status == ProductStatus.ReadyForDistribution)
                    .Sum(p => p.Price)
            })
            .OrderByDescending(w => w.TotalCost)
            .Select(w => w.Warehouse)
            .FirstAsync();

        return warehouse;
    }

    public async Task<IEnumerable<Warehouse>> GetWarehousesAsync()
    {
        var warehouses = await _dbContext.Warehouses
            .Where(w => w.Products.Any(p =>
                p.ReceivedDate >= StartReceivedDate &&
                p.ReceivedDate <= EndReceivedDate))
            .ToListAsync();

        return warehouses;
    }
}
