using System.Net;
using FluentValidation;

public class ErrorHandlingMiddleware : IMiddleware
{
    private readonly ILogger<ErrorHandlingMiddleware> _logger;

    public ErrorHandlingMiddleware(ILogger<ErrorHandlingMiddleware> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (ValidationException)
        {
            // Estas ya las maneja ValidationMiddleware → las ignoramos aquí.
            throw;
        }
        catch (Exception ex)
        {
            // Log interno para depuración
            _logger.LogError(ex, "Unhandled exception caught by middleware.");

            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";

            var problem = new
            {
                error = "Ocurrió un error inesperado en el servidor.",
                details = ex.Message
            };

            await context.Response.WriteAsJsonAsync(problem);
        }
    }
}
