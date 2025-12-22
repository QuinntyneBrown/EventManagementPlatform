// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace EventManagementPlatform.Core.Model.CustomerAggregate;

public class Customer
{
    public Guid CustomerId { get; set; }
    public string CompanyName { get; set; } = string.Empty;
    public CustomerType Type { get; set; } = CustomerType.Individual;
    public string PrimaryEmail { get; set; } = string.Empty;
    public string PrimaryPhone { get; set; } = string.Empty;
    public string? Industry { get; set; }
    public string? Website { get; set; }
    public string BillingStreet { get; set; } = string.Empty;
    public string BillingCity { get; set; } = string.Empty;
    public string BillingState { get; set; } = string.Empty;
    public string BillingPostalCode { get; set; } = string.Empty;
    public string BillingCountry { get; set; } = string.Empty;
    public CustomerStatus Status { get; set; } = CustomerStatus.Active;
    public bool IsDeleted { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ModifiedAt { get; set; }
    public Guid CreatedBy { get; set; }
    public Guid? ModifiedBy { get; set; }
}
