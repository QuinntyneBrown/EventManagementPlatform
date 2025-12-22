// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using EventManagementPlatform.Core.Model.CustomerAggregate;
using MediatR;

namespace EventManagementPlatform.Api.Features.Customers.UpdateCustomer;

public record UpdateCustomerCommand(
    Guid CustomerId,
    string CompanyName,
    CustomerType Type,
    string PrimaryEmail,
    string PrimaryPhone,
    string? Industry,
    string? Website,
    string BillingStreet,
    string BillingCity,
    string BillingState,
    string BillingPostalCode,
    string BillingCountry)  : IRequest<UpdateCustomerResponse>;
