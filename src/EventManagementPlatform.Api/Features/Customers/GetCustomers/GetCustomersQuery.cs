// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using EventManagementPlatform.Core.Model.CustomerAggregate;
using MediatR;

namespace EventManagementPlatform.Api.Features.Customers.GetCustomers;

public record GetCustomersQuery(
    int Page = 1,
    int PageSize = 20,
    CustomerStatus? Status = null,
    string? Search = null)  : IRequest<GetCustomersResponse>;
