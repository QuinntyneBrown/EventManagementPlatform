// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using EventManagementPlatform.Core.Model.CustomerAggregate;
using MediatR;

namespace EventManagementPlatform.Api.Features.Customers.CreateCustomer;

public record CreateCustomerCommand(
    string Company,
    CustomerType Type,
    string Email,
    string Phone,
    string? Industry,
    string? Website,
    string Address,
    string City,
    string State,
    string PostalCode,
    string BillingCountry = "Canada")  : IRequest<CreateCustomerResponse>;
