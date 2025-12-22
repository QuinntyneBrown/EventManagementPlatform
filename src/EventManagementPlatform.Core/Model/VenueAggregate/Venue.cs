// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace EventManagementPlatform.Core.Model.VenueAggregate;

public class Venue
{
    public Guid VenueId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public VenueType Type { get; set; } = VenueType.Other;

    public string Street { get; set; } = string.Empty;

    public string City { get; set; } = string.Empty;

    public string State { get; set; } = string.Empty;

    public string PostalCode { get; set; } = string.Empty;

    public string Country { get; set; } = string.Empty;

    public int MaxCapacity { get; set; }

    public int? SeatedCapacity { get; set; }

    public int? StandingCapacity { get; set; }

    public string? ContactName { get; set; }

    public string? ContactEmail { get; set; }

    public string? ContactPhone { get; set; }

    public VenueStatus Status { get; set; } = VenueStatus.Active;

    public bool IsDeleted { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public Guid CreatedBy { get; set; }

    public Guid? ModifiedBy { get; set; }
}
