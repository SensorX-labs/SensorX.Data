using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SensorX.Data.Domain.Common.Exceptions;

namespace SensorX.Data.WebApi.Configurations;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var (statusCode, title, detail, logLevel) = exception switch
        {
            DomainException => (
                StatusCodes.Status422UnprocessableEntity,
                "Business Rule Violation",
                exception.Message,
                LogLevel.Warning),
            SensorX.Data.Application.Common.Exceptions.ApplicationException => (
                StatusCodes.Status400BadRequest,
                "Application Error",
                exception.Message,
                LogLevel.Warning),
            _ => (
                StatusCodes.Status500InternalServerError,
                "Internal Server Error",
                "An unexpected error occurred.",
                LogLevel.Error),
        };

        logger.Log(logLevel, exception, "Exception occurred: {Message}", exception.Message);

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = detail,
            Instance = $"{httpContext.Request.Method} {httpContext.Request.Path}"
        };

        httpContext.Response.StatusCode = statusCode;

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}
