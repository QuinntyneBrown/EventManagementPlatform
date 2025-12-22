// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using FluentValidation;

namespace EventManagementPlatform.Api.Features.Equipment.CreateEquipmentItem;

public class CreateEquipmentItemCommandValidator : AbstractValidator<CreateEquipmentItemCommand>
{
    public CreateEquipmentItemCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(200).WithMessage("Name must not exceed 200 characters");

        RuleFor(x => x.Description)
            .MaximumLength(2000).WithMessage("Description must not exceed 2000 characters");

        RuleFor(x => x.PurchaseDate)
            .NotEmpty().WithMessage("Purchase date is required")
            .LessThanOrEqualTo(DateTime.UtcNow.Date).WithMessage("Purchase date cannot be in the future");

        RuleFor(x => x.PurchasePrice)
            .GreaterThan(0).WithMessage("Purchase price must be greater than 0");

        RuleFor(x => x.CurrentValue)
            .GreaterThanOrEqualTo(0).When(x => x.CurrentValue.HasValue)
            .WithMessage("Current value must be greater than or equal to 0");

        RuleFor(x => x.Manufacturer)
            .MaximumLength(200).WithMessage("Manufacturer must not exceed 200 characters");

        RuleFor(x => x.Model)
            .MaximumLength(200).WithMessage("Model must not exceed 200 characters");

        RuleFor(x => x.SerialNumber)
            .MaximumLength(100).WithMessage("Serial number must not exceed 100 characters");

        RuleFor(x => x.WarehouseLocation)
            .MaximumLength(100).WithMessage("Warehouse location must not exceed 100 characters");
    }
}
