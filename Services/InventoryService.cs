using backend.DTOs;
using backend.Models;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services;

public class InventoryService : IInventoryService
{
    private readonly StoreContext _context;

    public InventoryService(StoreContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<InventoryDto>> Get()
    {
        return await _context.Inventories
            .AsNoTracking()
            .Select(inventory => ToDto(inventory))
            .ToListAsync();
    }

    public async Task<InventoryDto?> GetById(int id)
    {
        var inventory = await _context.Inventories
            .AsNoTracking()
            .FirstOrDefaultAsync(item => item.InventoryId == id);

        return inventory is null ? null : ToDto(inventory);
    }

    public async Task<InventoryDto?> Add(InventoryInsertDto inventoryInsertDto)
    {
        if (!await _context.Beers.AnyAsync(beer => beer.BeerId == inventoryInsertDto.BeerId))
        {
            return null;
        }

        var inventory = new Inventory
        {
            Quantity = inventoryInsertDto.Quantity,
            BatchNumber = inventoryInsertDto.BatchNumber,
            BeerId = inventoryInsertDto.BeerId,
            ProductionDate = inventoryInsertDto.ProductionDate,
            PurchasePrice = inventoryInsertDto.PurchasePrice,
            StorageSection = inventoryInsertDto.StorageSection,
            BestBeforeDate = inventoryInsertDto.BestBeforeDate
        };

        await _context.Inventories.AddAsync(inventory);
        await _context.SaveChangesAsync();

        return ToDto(inventory);
    }

    public async Task<InventoryDto?> Update(int id, InventoryUpdateDto inventoryUpdateDto)
    {
        var inventory = await _context.Inventories.FindAsync(id);
        if (inventory is null)
        {
            return null;
        }

        if (!await _context.Beers.AnyAsync(beer => beer.BeerId == inventoryUpdateDto.BeerId))
        {
            return null;
        }

        inventory.Quantity = inventoryUpdateDto.Quantity;
        inventory.BatchNumber = inventoryUpdateDto.BatchNumber;
        inventory.BeerId = inventoryUpdateDto.BeerId;
        inventory.ProductionDate = inventoryUpdateDto.ProductionDate;
        inventory.PurchasePrice = inventoryUpdateDto.PurchasePrice;
        inventory.StorageSection = inventoryUpdateDto.StorageSection;
        inventory.BestBeforeDate = inventoryUpdateDto.BestBeforeDate;

        await _context.SaveChangesAsync();
        return ToDto(inventory);
    }

    public async Task<InventoryDto?> Delete(int id)
    {
        var inventory = await _context.Inventories.FindAsync(id);
        if (inventory is null)
        {
            return null;
        }

        _context.Inventories.Remove(inventory);
        await _context.SaveChangesAsync();
        return ToDto(inventory);
    }

    private static InventoryDto ToDto(Inventory inventory) => new()
    {
        InventoryId = inventory.InventoryId,
        Quantity = inventory.Quantity,
        BatchNumber = inventory.BatchNumber,
        BeerId = inventory.BeerId,
        ProductionDate = inventory.ProductionDate,
        PurchasePrice = inventory.PurchasePrice,
        StorageSection = inventory.StorageSection,
        BestBeforeDate = inventory.BestBeforeDate
    };
}
