using backend.DTOs;
using Backend.Models;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Backend.Validators;

public class SaleUpdateValidator : AbstractValidator<SaleUpdateDto>
{
    public SaleUpdateValidator(StoreContext context)
    {
        RuleFor(sale => sale.InventoryId)
            .GreaterThan(0).WithMessage("El lote de inventario es obligatorio.")
            .MustAsync((id, cancellationToken) =>
                context.Inventories.AnyAsync(inventory => inventory.InventoryId == id, cancellationToken))
            .WithMessage("El lote de inventario indicado no existe.");

        RuleFor(sale => sale.Quantity)
            .GreaterThan(0).WithMessage("La cantidad debe ser mayor que 0.");

        RuleFor(sale => sale.UnitPrice)
            .InclusiveBetween(0.01m, 9999999999999999.99m)
            .WithMessage("El precio unitario debe ser mayor que 0 y caber en decimal(18,2).");

        RuleFor(sale => sale.SaleDate)
            .NotEmpty().WithMessage("La fecha de venta es obligatoria.");

        RuleFor(sale => sale.ClientName)
            .NotEmpty().WithMessage("El nombre del cliente es obligatorio.")
            .Length(2, 50).WithMessage("El nombre del cliente debe tener entre 2 y 50 caracteres.");
    }
}
