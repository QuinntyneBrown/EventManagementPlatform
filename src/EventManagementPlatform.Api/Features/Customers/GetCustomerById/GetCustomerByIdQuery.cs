// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using MediatR;

namespace EventManagementPlatform.Api.Features.Customers.GetCustomerById;

public record GetCustomerByIdQuery(Guid CustomerId): IRequest<GetCustomerByIdResponse>;
