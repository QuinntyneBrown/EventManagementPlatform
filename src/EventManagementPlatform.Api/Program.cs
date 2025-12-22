// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using EventManagementPlatform.Api.Logging;
using EventManagementPlatform.Infrastructure.Services;
using Serilog;

// Configure Serilog early to capture startup errors
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    Log.Information("Starting Event Management Platform API");

    var builder = WebApplication.CreateBuilder(args);

    // Configure Serilog from appsettings.json
    builder.Host.UseSerilog((context, services, configuration) => configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext());

    // Add HttpContextAccessor for UserIdEnricher
    builder.Services.AddHttpContextAccessor();

    builder.Services.AddApiServices(builder.Configuration);

    var app = builder.Build();

    // Exception handling should be first
    app.UseExceptionHandling();

    // Correlation ID middleware
    app.UseCorrelationId();

    // Request logging with Serilog
    app.UseSerilogRequestLogging(options =>
    {
        options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
        {
            diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
            diagnosticContext.Set("RequestScheme", httpContext.Request.Scheme);
            diagnosticContext.Set("UserAgent", httpContext.Request.Headers.UserAgent.FirstOrDefault());
            diagnosticContext.Set("ClientIp", httpContext.Connection.RemoteIpAddress?.ToString());
        };
    });

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();

        // Seed development data
        using var scope = app.Services.CreateScope();
        var seedService = scope.ServiceProvider.GetRequiredService<ISeedService>();
        await seedService.SeedAsync();
    }

    app.UseHttpsRedirection();

    app.UseCors("CorsPolicy");

    app.UseAuthentication();

    app.UseAuthorization();

    app.MapControllers();

    Log.Information("Event Management Platform API started successfully");

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
