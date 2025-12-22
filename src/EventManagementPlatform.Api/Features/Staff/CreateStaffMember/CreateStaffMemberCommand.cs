// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using EventManagementPlatform.Core.Model.StaffAggregate;
using MediatR;

namespace EventManagementPlatform.Api.Features.Staff.CreateStaffMember;

public record CreateStaffMemberCommand(
    string FirstName,
    string LastName,
    string Email,
    string PhoneNumber,
    DateTime HireDate,
    StaffRole Role,
    decimal? HourlyRate) : IRequest<CreateStaffMemberResponse>;
