// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using EventManagementPlatform.Core.Model.VenueAggregate;
using MediatR;

namespace EventManagementPlatform.Api.Features.Venues.CreateVenue;

public record CreateVenueCommand(
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
    string? ContactPhone): IRequest<CreateVenueResponse>;
