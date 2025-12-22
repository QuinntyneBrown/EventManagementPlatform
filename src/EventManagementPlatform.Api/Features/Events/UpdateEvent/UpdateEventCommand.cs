// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using MediatR;

namespace EventManagementPlatform.Api.Features.Events.UpdateEvent;

public record UpdateEventCommand(
    Guid EventId,
    string Title,
    string? Description,
    DateTime EventDate,
    Guid VenueId,
    Guid EventTypeId,
    Guid CustomerId)  : IRequest<UpdateEventResponse>;
