// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using EventManagementPlatform.Core.Model.EventAggregate;
using MediatR;

namespace EventManagementPlatform.Api.Features.Events.UpdateEventStatus;

public record UpdateEventStatusCommand(Guid EventId, EventStatus Status) : IRequest<UpdateEventStatusResponse>;
