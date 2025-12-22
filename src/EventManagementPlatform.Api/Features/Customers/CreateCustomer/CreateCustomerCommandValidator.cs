// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using FluentValidation;

namespace EventManagementPlatform.Api.Features.Customers.CreateCustomer;

public class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
{
    public CreateCustomerCommandValidator()
    {
        RuleFor(x => x.CompanyName)
            .NotEmpty().WithMessage("Company name is required")
            .MaximumLength(200).WithMessage("Company name must not exceed 200 characters");

        RuleFor(x => x.PrimaryEmail)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email format")
            .MaximumLength(255).WithMessage("Email must not exceed 255 characters");

        RuleFor(x => x.PrimaryPhone)
            .NotEmpty().WithMessage("Phone is required")
            .MaximumLength(50).WithMessage("Phone must not exceed 50 characters");

        RuleFor(x => x.BillingStreet)
            .MaximumLength(200).WithMessage("Street must not exceed 200 characters");

        RuleFor(x => x.BillingCity)
            .MaximumLength(100).WithMessage("City must not exceed 100 characters");

        RuleFor(x => x.BillingState)
            .MaximumLength(100).WithMessage("State must not exceed 100 characters");

        RuleFor(x => x.BillingZipCode)
            .MaximumLength(20).WithMessage("Zip code must not exceed 20 characters");

        RuleFor(x => x.BillingCountry)
            .MaximumLength(100).WithMessage("Country must not exceed 100 characters");
    }
}
