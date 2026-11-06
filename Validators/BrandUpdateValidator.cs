using Backend.DTOs;
using FluentValidation;

namespace Backend.Validators
{
    public class BrandUpdateValidator : AbstractValidator<BrandUpdateDto>
    {
        public BrandUpdateValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("El Id debe ser mayor a 0");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("El nombre es obligatorio");

            RuleFor(x => x.Name)
                .Length(2, 20).WithMessage("El nombre debe medir de 2 a 20 caracteres");
        }
    }
}
