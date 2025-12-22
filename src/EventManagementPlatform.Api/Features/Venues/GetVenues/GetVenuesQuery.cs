// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using EventManagementPlatform.Core.Model.VenueAggregate;
using MediatR;

namespace EventManagementPlatform.Api.Features.Venues.GetVenues;

public record GetVenuesQuery(
    int Page = 1,
    int PageSize = 20,
    VenueStatus? Status = null,
    VenueType? Type = null,
    string? City = null,
    int? MinCapacity = null)  : IRequest<GetVenuesResponse>;
