// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace EventManagementPlatform.Core.Model.EventAggregate;

[System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1716:Identifiers should not match keywords", Justification = "Event is the domain model name")]
public class Event
{
    public Guid EventId { get; set; }

    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public DateTime EventDate { get; set; }

    public Guid VenueId { get; set; }

    public Guid EventTypeId { get; set; }

    public Guid CustomerId { get; set; }

    public EventStatus Status { get; set; } = EventStatus.Planned;

    public bool IsDeleted { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public Guid CreatedBy { get; set; }

    public Guid? ModifiedBy { get; set; }

    public EventType? EventType { get; set; }
}
