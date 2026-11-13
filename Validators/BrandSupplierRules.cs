using Backend.Models;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Backend.Validators;

internal static class BrandSupplierRules
{
    public static IRuleBuilderOptions<T, int> MustReferenceSupplier<T>(
        this IRuleBuilder<T, int> ruleBuilder,
        StoreContext context)
    {
        return ruleBuilder
            .GreaterThan(0).WithMessage("El proveedor es obligatorio.")
            .MustAsync((id, cancellationToken) =>
                context.Suppliers.AnyAsync(supplier => supplier.SupplierId == id, cancellationToken))
            .WithMessage("El proveedor indicado no existe.");
    }
}
