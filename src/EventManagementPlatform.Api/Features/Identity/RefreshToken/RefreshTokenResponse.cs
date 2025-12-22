// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace EventManagementPlatform.Api.Features.Identity.RefreshToken;

public record RefreshTokenResponse(string AccessToken, string RefreshToken);
