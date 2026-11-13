using backend.DTOs;
using Backend.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SaleController : ControllerBase
{
    private readonly ICommonService<SaleDto, SaleInsertDto, SaleUpdateDto> _saleService;
    private readonly IValidator<SaleInsertDto> _insertValidator;
    private readonly IValidator<SaleUpdateDto> _updateValidator;

    public SaleController(
        [FromKeyedServices("saleService")]
        ICommonService<SaleDto, SaleInsertDto, SaleUpdateDto> saleService,
        IValidator<SaleInsertDto> insertValidator,
        IValidator<SaleUpdateDto> updateValidator)
    {
        _saleService = saleService;
        _insertValidator = insertValidator;
        _updateValidator = updateValidator;
    }

    [HttpGet]
    public async Task<IEnumerable<SaleDto>> Get() =>
        await _saleService.Get();

    [HttpGet("{id:int}")]
    public async Task<ActionResult<SaleDto>> GetById(int id)
    {
        var sale = await _saleService.GetById(id);
        return sale is null ? NotFound() : Ok(sale);
    }

    [HttpPost]
    public async Task<ActionResult<SaleDto>> Add(SaleInsertDto saleInsertDto)
    {
        var validation = await _insertValidator.ValidateAsync(saleInsertDto);
        if (!validation.IsValid)
        {
            return BadRequest(validation.Errors);
        }

        var sale = await _saleService.Add(saleInsertDto);
        return CreatedAtAction(nameof(GetById), new { id = sale.SaleId }, sale);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<SaleDto>> Update(int id, SaleUpdateDto saleUpdateDto)
    {
        var validation = await _updateValidator.ValidateAsync(saleUpdateDto);
        if (!validation.IsValid)
        {
            return BadRequest(validation.Errors);
        }

        var sale = await _saleService.Update(id, saleUpdateDto);
        return sale is null ? NotFound() : Ok(sale);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<SaleDto>> Delete(int id)
    {
        var sale = await _saleService.Delete(id);
        return sale is null ? NotFound() : Ok(sale);
    }
}
