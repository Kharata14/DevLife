using System.Net;
using System.Text.Json;
using FluentValidation;

namespace DevLife.Api.Middleware;
public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
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

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var code = HttpStatusCode.InternalServerError; // 500
        var errors = new List<string> { "An unexpected error occurred." };

        switch (exception)
        {
            case ValidationException validationException:
                code = HttpStatusCode.BadRequest; // 400
                errors = validationException.Errors.Select(e => e.ErrorMessage).ToList();
                break;
            case ArgumentException argumentException:
                code = HttpStatusCode.Conflict; // 409
                errors = new List<string> { argumentException.Message };
                break;
        }

        var result = JsonSerializer.Serialize(new { errors });
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)code;
        return context.Response.WriteAsync(result);
    }
}
