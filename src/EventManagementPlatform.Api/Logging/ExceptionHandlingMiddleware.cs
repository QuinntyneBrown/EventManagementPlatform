// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Net;
using System.Text.Json;
using FluentValidation;
using Serilog;
using Serilog.Events;

namespace EventManagementPlatform.Api.Logging;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly Serilog.ILogger _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
        _logger = Log.ForContext<ExceptionHandlingMiddleware>();
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var (statusCode, message, logLevel) = exception switch
        {
            ValidationException validationEx => (
                HttpStatusCode.BadRequest,
                FormatValidationErrors(validationEx),
                LogEventLevel.Warning),

            KeyNotFoundException => (
                HttpStatusCode.NotFound,
                "The requested resource was not found.",
                LogEventLevel.Information),

            UnauthorizedAccessException => (
                HttpStatusCode.Unauthorized,
                "You are not authorized to access this resource.",
                LogEventLevel.Warning),

            InvalidOperationException invalidOpEx => (
                HttpStatusCode.BadRequest,
                invalidOpEx.Message,
                LogEventLevel.Warning),

            _ => (
                HttpStatusCode.InternalServerError,
                "An unexpected error occurred. Please try again later.",
                LogEventLevel.Error)
        };

        LogException(exception, logLevel, context);

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var response = new
        {
            error = message,
            correlationId = context.Items["X-Correlation-Id"]?.ToString()
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }

    private void LogException(Exception exception, LogEventLevel level, HttpContext context)
    {
        var requestPath = context.Request.Path;
        var requestMethod = context.Request.Method;

        switch (level)
        {
            case LogEventLevel.Warning:
                _logger.Warning(
                    exception,
                    "Request {Method} {Path} failed with warning: {ErrorMessage}",
                    requestMethod,
                    requestPath,
                    exception.Message);
                break;

            case LogEventLevel.Error:
                _logger.Error(
                    exception,
                    "Request {Method} {Path} failed with error: {ErrorMessage}",
                    requestMethod,
                    requestPath,
                    exception.Message);
                break;

            case LogEventLevel.Fatal:
                _logger.Fatal(
                    exception,
                    "Critical failure in request {Method} {Path}: {ErrorMessage}",
                    requestMethod,
                    requestPath,
                    exception.Message);
                break;

            default:
                _logger.Information(
                    "Request {Method} {Path} completed with status: {ErrorMessage}",
                    requestMethod,
                    requestPath,
                    exception.Message);
                break;
        }
    }

    private static string FormatValidationErrors(ValidationException exception)
    {
        var errors = exception.Errors
            .GroupBy(e => e.PropertyName)
            .ToDictionary(
                g => g.Key,
                g => g.Select(e => e.ErrorMessage).ToArray());

        return JsonSerializer.Serialize(new { validationErrors = errors });
    }
}

public static class ExceptionHandlingMiddlewareExtensions
{
    public static IApplicationBuilder UseExceptionHandling(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ExceptionHandlingMiddleware>();
    }
}
