// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace EventManagementPlatform.Core.Model.StaffAggregate;

public class StaffMember
{
    public Guid StaffMemberId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string? PhotoUrl { get; set; }
    public StaffStatus Status { get; set; } = StaffStatus.Active;
    public DateTime HireDate { get; set; }
    public DateTime? TerminationDate { get; set; }
    public StaffRole Role { get; set; } = StaffRole.Other;
    public decimal? HourlyRate { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ModifiedAt { get; set; }
    public Guid CreatedBy { get; set; }
    public Guid? ModifiedBy { get; set; }
}
