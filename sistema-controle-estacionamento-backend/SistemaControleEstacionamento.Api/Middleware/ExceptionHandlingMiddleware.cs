using System.Net;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using SistemaControleEstacionamento.Domain.Exceptions;

namespace SistemaControleEstacionamento.Api.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
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
        var statusCode = HttpStatusCode.InternalServerError;
        var message = "Ocorreu um erro interno no servidor.";

        switch (exception)
        {
            case ConcurrencyException:
                statusCode = HttpStatusCode.Conflict;
                message = exception.Message;
                _logger.LogWarning(exception, "Conflito de concorrência detectado");
                break;

            case BusinessException:
                statusCode = HttpStatusCode.UnprocessableEntity;
                message = exception.Message;
                _logger.LogWarning(exception, "Violação de regra de negócio");
                break;

            case NotFoundException:
                statusCode = HttpStatusCode.NotFound;
                message = exception.Message;
                _logger.LogWarning(exception, "Recurso não encontrado");
                break;

            case DbUpdateConcurrencyException:
                statusCode = HttpStatusCode.Conflict;
                message = "O recurso foi modificado por outro usuário. Busque os dados atualizados e tente novamente.";
                _logger.LogWarning(exception, "Conflito de concorrência no banco de dados");
                break;

            case DbUpdateException dbEx when dbEx.InnerException?.Message.Contains("UNIQUE") == true:
                statusCode = HttpStatusCode.Conflict;
                message = "Já existe um registro com essas informações.";
                _logger.LogWarning(exception, "Violação de constraint UNIQUE");
                break;

            default:
                _logger.LogError(exception, "Erro não tratado");
                break;
        }

        var response = new
        {
            error = message,
            statusCode = (int)statusCode,
            timestamp = DateTime.UtcNow,
            path = context.Request.Path.Value
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        await context.Response.WriteAsync(JsonSerializer.Serialize(response, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        }));
    }
}
