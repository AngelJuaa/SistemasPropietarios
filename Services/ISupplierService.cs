namespace Backend.Services;

public interface ISupplierService : ICommonService<
    backend.DTOs.SupplierDto,
    backend.DTOs.SupplierInsertDto,
    backend.DTOs.SupplierUpdateDto>
{
}
