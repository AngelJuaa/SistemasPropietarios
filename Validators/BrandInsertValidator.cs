using Backend.DTOs;
using Backend.Models;
using FluentValidation;

namespace Backend.Validators
{
    public class BrandInsertValidator : AbstractValidator<BrandInsertDto>
    {
        public BrandInsertValidator(StoreContext context)
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("El nombre es obligatorio");

            RuleFor(x => x.Name)
                .Length(2, 20).WithMessage("El nombre debe medir de 2 a 20 caracteres");

            RuleFor(x => x.SupplierId).MustReferenceSupplier(context);
        }
    }
}
