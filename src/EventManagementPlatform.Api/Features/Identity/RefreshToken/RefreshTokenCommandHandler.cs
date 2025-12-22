// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using EventManagementPlatform.Core;
using EventManagementPlatform.Core.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EventManagementPlatform.Api.Features.Identity.RefreshToken;

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, RefreshTokenResponse>
{
    private readonly IEventManagementPlatformContext _context;
    private readonly ITokenService _tokenService;

    public RefreshTokenCommandHandler(
        IEventManagementPlatformContext context,
        ITokenService tokenService)
    {
        _context = context;
        _tokenService = tokenService;
    }

    public async Task<RefreshTokenResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .Include(x => x.Roles)
            .FirstOrDefaultAsync(x => x.RefreshToken == request.RefreshToken, cancellationToken);

        if (user == null)
        {
            throw new UnauthorizedAccessException("Invalid refresh token");
        }

        var accessToken = _tokenService.GenerateAccessToken(user);
        var newRefreshToken = _tokenService.GenerateRefreshToken();

        user.RefreshToken = newRefreshToken;
        await _context.SaveChangesAsync(cancellationToken);

        return new RefreshTokenResponse(accessToken, newRefreshToken);
    }
}
