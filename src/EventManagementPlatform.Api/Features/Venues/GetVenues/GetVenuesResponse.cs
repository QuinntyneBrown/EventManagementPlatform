// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace EventManagementPlatform.Api.Features.Venues.GetVenues;

public record GetVenuesResponse(IEnumerable<VenueDto> Venues, int TotalCount, int Page, int PageSize);
