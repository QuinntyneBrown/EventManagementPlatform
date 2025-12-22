// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using EventManagementPlatform.Core.Model.EventAggregate;
using MediatR;

namespace EventManagementPlatform.Api.Features.Events.GetEvents;

public record GetEventsQuery(
    int Page = 1,
    int PageSize = 20,
    EventStatus? Status = null,
    Guid? VenueId = null,
    Guid? CustomerId = null): IRequest<GetEventsResponse>;
