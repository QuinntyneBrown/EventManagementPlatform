// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using EventManagementPlatform.Core.Model.VenueAggregate;

namespace EventManagementPlatform.Api.Features.Venues;

public record VenueDto(
    Guid VenueId,
    string Name,
    string? Description,
    VenueType Type,
    string Street,
    string City,
    string State,
    string PostalCode,
    string Country,
    int MaxCapacity,
    int? SeatedCapacity,
    int? StandingCapacity,
    string? ContactName,
    string? ContactEmail,
    string? ContactPhone,
    VenueStatus Status,
    DateTime CreatedAt,
    DateTime? ModifiedAt);
