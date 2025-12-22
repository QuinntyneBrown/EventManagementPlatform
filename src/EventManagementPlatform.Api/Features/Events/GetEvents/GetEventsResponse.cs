// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace EventManagementPlatform.Api.Features.Events.GetEvents;

public record GetEventsResponse(IEnumerable<EventDto> Events, int TotalCount, int Page, int PageSize);
