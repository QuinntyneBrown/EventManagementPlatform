// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using EventManagementPlatform.Core.Model.StaffAggregate;
using MediatR;

namespace EventManagementPlatform.Api.Features.Staff.GetStaffMembers;

public record GetStaffMembersQuery(
    int Page = 1,
    int PageSize = 20,
    StaffStatus? Status = null,
    StaffRole? Role = null,
    string? Search = null)  : IRequest<GetStaffMembersResponse>;
