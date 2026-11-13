using backend.DTOs;
using Backend.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SupplierController : ControllerBase
{
    private readonly ICommonService<SupplierDto, SupplierInsertDto, SupplierUpdateDto> _supplierService;
    private readonly IValidator<SupplierInsertDto> _insertValidator;
    private readonly IValidator<SupplierUpdateDto> _updateValidator;

    public SupplierController(
        [FromKeyedServices("supplierService")]
        ICommonService<SupplierDto, SupplierInsertDto, SupplierUpdateDto> supplierService,
        IValidator<SupplierInsertDto> insertValidator,
        IValidator<SupplierUpdateDto> updateValidator)
    {
        _supplierService = supplierService;
        _insertValidator = insertValidator;
        _updateValidator = updateValidator;
    }

    [HttpGet]
    public async Task<IEnumerable<SupplierDto>> Get() =>
        await _supplierService.Get();

    [HttpGet("{id:int}")]
    public async Task<ActionResult<SupplierDto>> GetById(int id)
    {
        var supplier = await _supplierService.GetById(id);
        return supplier is null ? NotFound() : Ok(supplier);
    }

    [HttpPost]
    public async Task<ActionResult<SupplierDto>> Add(SupplierInsertDto supplierInsertDto)
    {
        var validation = await _insertValidator.ValidateAsync(supplierInsertDto);
        if (!validation.IsValid)
        {
            return BadRequest(validation.Errors);
        }

        var supplier = await _supplierService.Add(supplierInsertDto);
        return CreatedAtAction(nameof(GetById), new { id = supplier.SupplierId }, supplier);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<SupplierDto>> Update(int id, SupplierUpdateDto supplierUpdateDto)
    {
        var validation = await _updateValidator.ValidateAsync(supplierUpdateDto);
        if (!validation.IsValid)
        {
            return BadRequest(validation.Errors);
        }

        var supplier = await _supplierService.Update(id, supplierUpdateDto);
        return supplier is null ? NotFound() : Ok(supplier);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<SupplierDto>> Delete(int id)
    {
        try
        {
            var supplier = await _supplierService.Delete(id);
            return supplier is null ? NotFound() : Ok(supplier);
        }
        catch (InvalidOperationException exception)
        {
            return Conflict(exception.Message);
        }
    }
}
