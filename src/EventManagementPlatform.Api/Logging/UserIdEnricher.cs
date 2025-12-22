// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Security.Claims;
using Serilog.Core;
using Serilog.Events;

namespace EventManagementPlatform.Api.Logging;

public class UserIdEnricher : ILogEventEnricher
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserIdEnricher(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext?.User?.Identity?.IsAuthenticated == true)
        {
            var userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                      ?? httpContext.User.FindFirst("sub")?.Value
                      ?? "Unknown";

            var property = propertyFactory.CreateProperty("UserId", userId);
            logEvent.AddPropertyIfAbsent(property);

            var username = httpContext.User.FindFirst(ClaimTypes.Name)?.Value
                        ?? httpContext.User.FindFirst("name")?.Value;
            if (!string.IsNullOrEmpty(username))
            {
                var usernameProperty = propertyFactory.CreateProperty("Username", username);
                logEvent.AddPropertyIfAbsent(usernameProperty);
            }
        }
        else
        {
            var property = propertyFactory.CreateProperty("UserId", "Anonymous");
            logEvent.AddPropertyIfAbsent(property);
        }
    }
}
