// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using EventManagementPlatform.Core.Model.CustomerAggregate;

namespace EventManagementPlatform.Api.Features.Customers;

public record CustomerDto(
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
    string BillingZipCode,
    string BillingCountry,
    CustomerStatus Status,
    DateTime CreatedAt,
    DateTime? ModifiedAt);
