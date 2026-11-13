using System;
using backend.DTOs;
using FluentValidation;

namespace backend.Validators;

public class InventoryInsertValidator : AbstractValidator<InventoryInsertDto>
{
    public InventoryInsertValidator()
    {
        RuleFor(x => x.Quantity).GreaterThan(0).WithMessage("La cantidad debe ser mayor que 0");
        RuleFor(x => x.BatchNumber).GreaterThan(0).WithMessage("El numero de lote debe ser mayor que 0");
        RuleFor(x => x.BeerId).GreaterThan(0).WithMessage("El identificador de la cerveza debe ser mayor que 0");
        RuleFor(x => x.ProductionDate).NotEmpty().WithMessage("La fecha de produccion es obligatoria");
        RuleFor(x => x.PurchasePrice).GreaterThan(0).WithMessage("El precio debe ser mayor a 0");
        RuleFor(x => x.BestBeforeDate).NotEmpty().WithMessage("la fecha de producto es obligatoria")
            .GreaterThan(x => x.ProductionDate).WithMessage("La fecha de consumo preferente debe ser posterior a la fecha de produccion");
        RuleFor(x => x.StorageSection).NotEmpty().WithMessage("La seccion de almacenamiento es obligatoria")
            .MaximumLength(100).WithMessage("La seccion de almacenamiento no puede superar 100 caracteres");
    }
}
