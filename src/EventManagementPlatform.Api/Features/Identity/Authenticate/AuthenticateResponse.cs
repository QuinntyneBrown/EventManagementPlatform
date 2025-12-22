// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace EventManagementPlatform.Api.Features.Identity.Authenticate;

public record AuthenticateResponse(
    Guid UserId,
    string Username,
    string AccessToken,
    string RefreshToken,
    IEnumerable<string> Roles);
