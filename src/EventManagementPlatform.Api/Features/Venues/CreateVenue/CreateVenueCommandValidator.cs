// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using FluentValidation;

namespace EventManagementPlatform.Api.Features.Venues.CreateVenue;

public class CreateVenueCommandValidator : AbstractValidator<CreateVenueCommand>
{
    public CreateVenueCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(200).WithMessage("Name must not exceed 200 characters");

        RuleFor(x => x.Description)
            .MaximumLength(2000).WithMessage("Description must not exceed 2000 characters");

        RuleFor(x => x.Street)
            .MaximumLength(200).WithMessage("Street must not exceed 200 characters");

        RuleFor(x => x.City)
            .MaximumLength(100).WithMessage("City must not exceed 100 characters");

        RuleFor(x => x.MaxCapacity)
            .GreaterThan(0).WithMessage("Max capacity must be greater than 0");

        RuleFor(x => x.ContactEmail)
            .EmailAddress().When(x => !string.IsNullOrEmpty(x.ContactEmail))
            .WithMessage("Invalid email format");
    }
}
