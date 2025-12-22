// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using FluentValidation;

namespace EventManagementPlatform.Api.Features.Staff.CreateStaffMember;

public class CreateStaffMemberCommandValidator : AbstractValidator<CreateStaffMemberCommand>
{
    public CreateStaffMemberCommandValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required")
            .MaximumLength(100).WithMessage("First name must not exceed 100 characters");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required")
            .MaximumLength(100).WithMessage("Last name must not exceed 100 characters");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email format")
            .MaximumLength(255).WithMessage("Email must not exceed 255 characters");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Phone number is required")
            .MaximumLength(50).WithMessage("Phone number must not exceed 50 characters");

        RuleFor(x => x.HireDate)
            .NotEmpty().WithMessage("Hire date is required")
            .LessThanOrEqualTo(DateTime.UtcNow.Date).WithMessage("Hire date cannot be in the future");

        RuleFor(x => x.HourlyRate)
            .GreaterThanOrEqualTo(0).When(x => x.HourlyRate.HasValue)
            .WithMessage("Hourly rate must be greater than or equal to 0");
    }
}
