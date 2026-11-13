using backend.DTOs;

namespace Backend.Services;

public interface IInventoryService
{
    Task<IEnumerable<InventoryDto>> Get();
    Task<InventoryDto?> GetById(int id);
    Task<InventoryDto?> Add(InventoryInsertDto inventoryInsertDto);
    Task<InventoryDto?> Update(int id, InventoryUpdateDto inventoryUpdateDto);
    Task<InventoryDto?> Delete(int id);
}
