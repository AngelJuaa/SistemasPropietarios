using backend.DTOs;
using Backend.Models;
using Backend.Repository;

namespace Backend.Services;

public class SaleService : ICommonService<SaleDto, SaleInsertDto, SaleUpdateDto>
{
    private readonly IRepository<Sale> _saleRepository;

    public SaleService(IRepository<Sale> saleRepository)
    {
        _saleRepository = saleRepository;
    }

    public async Task<IEnumerable<SaleDto>> Get()
    {
        var sales = await _saleRepository.Get();
        return sales.Select(ToDto);
    }

    public async Task<SaleDto?> GetById(int id)
    {
        var sale = await _saleRepository.GetById(id);
        return sale is null ? null : ToDto(sale);
    }

    public async Task<SaleDto> Add(SaleInsertDto saleInsertDto)
    {
        var sale = new Sale
        {
            InventoryId = saleInsertDto.InventoryId,
            Quantity = saleInsertDto.Quantity,
            UnitPrice = saleInsertDto.UnitPrice,
            SaleDate = saleInsertDto.SaleDate,
            ClientName = saleInsertDto.ClientName.Trim()
        };

        await _saleRepository.Add(sale);
        await _saleRepository.Save();
        return ToDto(sale);
    }

    public async Task<SaleDto?> Update(int id, SaleUpdateDto saleUpdateDto)
    {
        var sale = await _saleRepository.GetById(id);
        if (sale is null)
        {
            return null;
        }

        sale.InventoryId = saleUpdateDto.InventoryId;
        sale.Quantity = saleUpdateDto.Quantity;
        sale.UnitPrice = saleUpdateDto.UnitPrice;
        sale.SaleDate = saleUpdateDto.SaleDate;
        sale.ClientName = saleUpdateDto.ClientName.Trim();

        _saleRepository.Update(sale);
        await _saleRepository.Save();
        return ToDto(sale);
    }

    public async Task<SaleDto?> Delete(int id)
    {
        var sale = await _saleRepository.GetById(id);
        if (sale is null)
        {
            return null;
        }

        _saleRepository.Delete(sale);
        await _saleRepository.Save();
        return ToDto(sale);
    }

    private static SaleDto ToDto(Sale sale) => new()
    {
        SaleId = sale.SaleId,
        InventoryId = sale.InventoryId,
        Quantity = sale.Quantity,
        UnitPrice = sale.UnitPrice,
        SaleDate = sale.SaleDate,
        ClientName = sale.ClientName
    };
}
