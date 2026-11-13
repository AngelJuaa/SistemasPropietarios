using backend.DTOs;
using Backend.Models;
using Backend.Repository;

namespace Backend.Services;

public class SupplierService :
    ICommonService<SupplierDto, SupplierInsertDto, SupplierUpdateDto>,
    ISupplierService
{
    private readonly IRepository<Supplier> _supplierRepository;
    private readonly IRepository<Brand> _brandRepository;

    public SupplierService(
        IRepository<Supplier> supplierRepository,
        IRepository<Brand> brandRepository)
    {
        _supplierRepository = supplierRepository;
        _brandRepository = brandRepository;
    }

    public async Task<IEnumerable<SupplierDto>> Get()
    {
        var suppliers = await _supplierRepository.Get();
        return suppliers.Select(ToDto);
    }

    public async Task<SupplierDto?> GetById(int id)
    {
        var supplier = await _supplierRepository.GetById(id);
        return supplier is null ? null : ToDto(supplier);
    }

    public async Task<SupplierDto> Add(SupplierInsertDto supplierInsertDto)
    {
        var supplier = new Supplier
        {
            Name = supplierInsertDto.Name.Trim(),
            Country = supplierInsertDto.Country.Trim(),
            Phone = supplierInsertDto.Phone.Trim()
        };

        await _supplierRepository.Add(supplier);
        await _supplierRepository.Save();
        return ToDto(supplier);
    }

    public async Task<SupplierDto?> Update(int id, SupplierUpdateDto supplierUpdateDto)
    {
        var supplier = await _supplierRepository.GetById(id);
        if (supplier is null)
        {
            return null;
        }

        supplier.Name = supplierUpdateDto.Name.Trim();
        supplier.Country = supplierUpdateDto.Country.Trim();
        supplier.Phone = supplierUpdateDto.Phone.Trim();

        _supplierRepository.Update(supplier);
        await _supplierRepository.Save();
        return ToDto(supplier);
    }

    public async Task<SupplierDto?> Delete(int id)
    {
        var supplier = await _supplierRepository.GetById(id);
        if (supplier is null)
        {
            return null;
        }

        if ((await _brandRepository.Get()).Any(brand => brand.SupplierId == id))
        {
            throw new InvalidOperationException("No se puede eliminar un proveedor con marcas asociadas.");
        }

        _supplierRepository.Delete(supplier);
        await _supplierRepository.Save();
        return ToDto(supplier);
    }

    private static SupplierDto ToDto(Supplier supplier) => new()
    {
        SupplierId = supplier.SupplierId,
        Name = supplier.Name,
        Country = supplier.Country,
        Phone = supplier.Phone
    };
}
