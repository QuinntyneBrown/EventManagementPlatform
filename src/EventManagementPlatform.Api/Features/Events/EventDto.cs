// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using EventManagementPlatform.Core.Model.EventAggregate;

namespace EventManagementPlatform.Api.Features.Events;

public record EventDto(
    Guid EventId,
    string Title,
    string? Description,
    DateTime EventDate,
    Guid VenueId,
    Guid EventTypeId,
    Guid CustomerId,
    EventStatus Status,
    DateTime CreatedAt,
    DateTime? ModifiedAt);
