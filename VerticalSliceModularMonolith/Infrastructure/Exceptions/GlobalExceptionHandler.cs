using Microsoft.AspNetCore.Diagnostics;
using System.Net;
using VerticalSliceModularMonolith.Shared.Exceptions;
using VerticalSliceModularMonolith.Shared.Utils;

namespace VerticalSliceModularMonolith.Infrastructure.Exceptions;

internal sealed class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        httpContext.Response.ContentType = "application/json";

        var errorMessage = JsonUtils.Serialize(new
        {
            error = "Tivemos um problema inesperado",
            message = "Tivemos um problema inesperado",
            description = "Por favor entre em contato com o suporte"
        });

        if (exception is BadRequestException)
        {
            httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            errorMessage = JsonUtils.Serialize(new
            {
                error = exception.Message,
                message = exception.Message,
            });
        }
        else
        {
            _logger.LogError(exception, "{message}", exception?.Message);
        }

        await httpContext.Response
            .WriteAsJsonAsync(errorMessage, cancellationToken);

        return true;
    }
}
