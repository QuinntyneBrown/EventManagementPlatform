// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Diagnostics;
using System.Text.Json;
using MediatR;
using Serilog;
using Serilog.Context;

namespace EventManagementPlatform.Api.Logging;

public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly ILogger _logger;
    private static readonly HashSet<string> SensitivePropertyNames = new(StringComparer.OrdinalIgnoreCase)
    {
        "password", "passwordhash", "token", "accesstoken", "refreshtoken",
        "secret", "apikey", "creditcard", "cardnumber", "cvv", "ssn",
        "socialsecuritynumber", "pin"
    };

    public LoggingBehavior()
    {
        _logger = Log.ForContext<LoggingBehavior<TRequest, TResponse>>();
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        var stopwatch = Stopwatch.StartNew();

        // Extract context identifiers from the request for log enrichment
        var contextProperties = ExtractContextIdentifiers(request);

        using (PushProperties(contextProperties))
        {
            _logger.Information(
                "Handling {RequestName} with {@Request}",
                requestName,
                SanitizeRequest(request));

            try
            {
                var response = await next();

                stopwatch.Stop();

                _logger.Information(
                    "Handled {RequestName} in {ElapsedMilliseconds}ms",
                    requestName,
                    stopwatch.ElapsedMilliseconds);

                return response;
            }
            catch (FluentValidation.ValidationException ex)
            {
                stopwatch.Stop();

                _logger.Warning(
                    ex,
                    "Validation failed for {RequestName} after {ElapsedMilliseconds}ms: {ValidationErrors}",
                    requestName,
                    stopwatch.ElapsedMilliseconds,
                    ex.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }));

                throw;
            }
            catch (Exception ex)
            {
                stopwatch.Stop();

                _logger.Error(
                    ex,
                    "Error handling {RequestName} after {ElapsedMilliseconds}ms: {ErrorMessage}",
                    requestName,
                    stopwatch.ElapsedMilliseconds,
                    ex.Message);

                throw;
            }
        }
    }

    private static Dictionary<string, object?> ExtractContextIdentifiers(TRequest request)
    {
        var properties = new Dictionary<string, object?>();
        var requestType = typeof(TRequest);

        // Look for common identifier properties
        var idProperties = new[] { "CustomerId", "EventId", "VenueId", "StaffId", "EquipmentId", "UserId" };

        foreach (var propName in idProperties)
        {
            var prop = requestType.GetProperty(propName);
            if (prop != null)
            {
                var value = prop.GetValue(request);
                if (value != null)
                {
                    properties[propName] = value;
                }
            }
        }

        return properties;
    }

    private static IDisposable PushProperties(Dictionary<string, object?> properties)
    {
        var disposables = new List<IDisposable>();
        foreach (var (key, value) in properties)
        {
            disposables.Add(LogContext.PushProperty(key, value));
        }

        return new CompositeDisposable(disposables);
    }

    private static object SanitizeRequest(TRequest request)
    {
        try
        {
            var properties = typeof(TRequest).GetProperties()
                .Where(p => p.CanRead)
                .ToDictionary(
                    p => p.Name,
                    p => SensitivePropertyNames.Contains(p.Name)
                        ? "[REDACTED]"
                        : p.GetValue(request)?.ToString() ?? "null");

            return properties;
        }
        catch
        {
            return new { Type = typeof(TRequest).Name, Message = "Unable to serialize request" };
        }
    }

    private class CompositeDisposable : IDisposable
    {
        private readonly List<IDisposable> _disposables;

        public CompositeDisposable(List<IDisposable> disposables)
        {
            _disposables = disposables;
        }

        public void Dispose()
        {
            foreach (var disposable in _disposables)
            {
                disposable.Dispose();
            }
        }
    }
}
