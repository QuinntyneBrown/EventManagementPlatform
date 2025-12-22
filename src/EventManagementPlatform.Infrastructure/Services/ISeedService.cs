// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace EventManagementPlatform.Infrastructure.Services;

public interface ISeedService
{
    Task SeedAsync(CancellationToken cancellationToken = default);
}
