// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using EventManagementPlatform.Core;
using EventManagementPlatform.Core.Model.UserAggregate;
using EventManagementPlatform.Core.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EventManagementPlatform.Api.Features.Identity.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, RegisterResponse>
{
    private readonly IEventManagementPlatformContext _context;
    private readonly IPasswordHasher _passwordHasher;

    public RegisterCommandHandler(
        IEventManagementPlatformContext context,
        IPasswordHasher passwordHasher)
    {
        _context = context;
        _passwordHasher = passwordHasher;
    }

    public async Task<RegisterResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await _context.Users
            .FirstOrDefaultAsync(x => x.Username == request.Username, cancellationToken);

        if (existingUser != null)
        {
            throw new InvalidOperationException("Username already exists");
        }

        var (hash, salt) = _passwordHasher.HashPassword(request.Password);

        var user = new User
        {
            UserId = Guid.NewGuid(),
            Username = request.Username,
            Password = hash,
            Salt = salt,
            CreatedAt = DateTime.UtcNow
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync(cancellationToken);

        return new RegisterResponse(user.UserId, user.Username);
    }
}
