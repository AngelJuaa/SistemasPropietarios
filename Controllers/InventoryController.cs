using backend.DTOs;
using Backend.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class InventoryController : ControllerBase
{
    private readonly IInventoryService _inventoryService;
    private readonly IValidator<InventoryInsertDto> _insertValidator;
    private readonly IValidator<InventoryUpdateDto> _updateValidator;

    public InventoryController(
        IInventoryService inventoryService,
        IValidator<InventoryInsertDto> insertValidator,
        IValidator<InventoryUpdateDto> updateValidator)
    {
        _inventoryService = inventoryService;
        _insertValidator = insertValidator;
        _updateValidator = updateValidator;
    }

    [HttpGet]
    public async Task<IEnumerable<InventoryDto>> Get() => await _inventoryService.Get();

    [HttpGet("{id:int}")]
    public async Task<ActionResult<InventoryDto>> GetById(int id)
    {
        var inventory = await _inventoryService.GetById(id);
        return inventory is null ? NotFound() : Ok(inventory);
    }

    [HttpPost]
    public async Task<ActionResult<InventoryDto>> Add(InventoryInsertDto inventoryInsertDto)
    {
        var validationResult = await _insertValidator.ValidateAsync(inventoryInsertDto);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        var inventory = await _inventoryService.Add(inventoryInsertDto);
        if (inventory is null)
        {
            return BadRequest("La cerveza indicada no existe.");
        }

        return CreatedAtAction(nameof(GetById), new { id = inventory.InventoryId }, inventory);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<InventoryDto>> Update(int id, InventoryUpdateDto inventoryUpdateDto)
    {
        var validationResult = await _updateValidator.ValidateAsync(inventoryUpdateDto);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        if (await _inventoryService.GetById(id) is null)
        {
            return NotFound();
        }

        var inventory = await _inventoryService.Update(id, inventoryUpdateDto);
        if (inventory is null)
        {
            return BadRequest("La cerveza indicada no existe.");
        }

        return Ok(inventory);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<InventoryDto>> Delete(int id)
    {
        var inventory = await _inventoryService.Delete(id);
        return inventory is null ? NotFound() : Ok(inventory);
    }
}
