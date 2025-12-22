// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using EventManagementPlatform.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EventManagementPlatform.Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<EventManagementPlatformDbContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? "Server=(localdb)\\mssqllocaldb;Database=EventManagementPlatform;Trusted_Connection=True;MultipleActiveResultSets=true";
            options.UseSqlServer(connectionString);
        });

        services.AddScoped<IEventManagementPlatformContext>(provider =>
            provider.GetRequiredService<EventManagementPlatformDbContext>());

        return services;
    }
}
