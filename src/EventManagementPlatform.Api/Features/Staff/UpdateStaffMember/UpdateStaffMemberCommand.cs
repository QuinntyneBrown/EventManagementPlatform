// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using EventManagementPlatform.Core.Model.StaffAggregate;
using MediatR;

namespace EventManagementPlatform.Api.Features.Staff.UpdateStaffMember;

public record UpdateStaffMemberCommand(
    Guid StaffMemberId,
    string FirstName,
    string LastName,
    string PhoneNumber,
    StaffRole Role,
    decimal? HourlyRate): IRequest<UpdateStaffMemberResponse>;
