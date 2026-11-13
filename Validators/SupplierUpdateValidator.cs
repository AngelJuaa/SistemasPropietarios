using backend.DTOs;
using FluentValidation;

namespace Backend.Validators;

public class SupplierUpdateValidator : AbstractValidator<SupplierUpdateDto>
{
    public SupplierUpdateValidator()
    {
        RuleFor(supplier => supplier.Name)
            .NotEmpty().WithMessage("El nombre es obligatorio.")
            .Length(2, 50).WithMessage("El nombre debe tener entre 2 y 50 caracteres.");

        RuleFor(supplier => supplier.Country)
            .NotEmpty().WithMessage("El país es obligatorio.")
            .Length(2, 50).WithMessage("El país debe tener entre 2 y 50 caracteres.");

        RuleFor(supplier => supplier.Phone)
            .NotEmpty().WithMessage("El teléfono es obligatorio.")
            .Length(7, 15).WithMessage("El teléfono debe tener entre 7 y 15 caracteres.")
            .Matches(@"^\+?[0-9]{7,15}$").WithMessage("El teléfono solo puede contener dígitos y un signo + inicial.");
    }
}
