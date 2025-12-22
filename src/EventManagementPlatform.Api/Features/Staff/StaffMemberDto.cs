// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using EventManagementPlatform.Core.Model.StaffAggregate;

namespace EventManagementPlatform.Api.Features.Staff;

public record StaffMemberDto(
    Guid StaffMemberId,
    string FirstName,
    string LastName,
    string Email,
    string PhoneNumber,
    string? PhotoUrl,
    StaffStatus Status,
    DateTime HireDate,
    DateTime? TerminationDate,
    StaffRole Role,
    decimal? HourlyRate,
    DateTime CreatedAt,
    DateTime? ModifiedAt);
