// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using FluentValidation;

namespace EventManagementPlatform.Api.Features.Events.UpdateEvent;

public class UpdateEventCommandValidator : AbstractValidator<UpdateEventCommand>
{
    public UpdateEventCommandValidator()
    {
        RuleFor(x => x.EventId)
            .NotEmpty().WithMessage("Event ID is required");

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .MaximumLength(200).WithMessage("Title must not exceed 200 characters");

        RuleFor(x => x.Description)
            .MaximumLength(2000).WithMessage("Description must not exceed 2000 characters");

        RuleFor(x => x.EventDate)
            .NotEmpty().WithMessage("Event date is required");

        RuleFor(x => x.VenueId)
            .NotEmpty().WithMessage("Venue is required");

        RuleFor(x => x.EventTypeId)
            .NotEmpty().WithMessage("Event type is required");

        RuleFor(x => x.CustomerId)
            .NotEmpty().WithMessage("Customer is required");
    }
}
