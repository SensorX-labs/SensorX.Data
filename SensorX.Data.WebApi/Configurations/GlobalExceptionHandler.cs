using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Domain.Common.Exceptions;

namespace SensorX.Data.WebApi.Configurations;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var (statusCode, message, logLevel) = exception switch
        {
            DomainException => (
                StatusCodes.Status422UnprocessableEntity,
                exception.Message,
                LogLevel.Warning),
            SensorX.Data.Application.Common.Exceptions.ApplicationException => (
                StatusCodes.Status400BadRequest,
                exception.Message,
                LogLevel.Warning),
            _ => (
                StatusCodes.Status500InternalServerError,
                exception.Message,
                LogLevel.Error),
        };

        logger.Log(logLevel, exception, "Exception occurred: {Message}", exception.Message);

        var result = Result.Failure(message);

        httpContext.Response.StatusCode = statusCode;

        await httpContext.Response.WriteAsJsonAsync(result, cancellationToken);

        return true;
    }
}
