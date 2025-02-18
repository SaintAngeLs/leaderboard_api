using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Leaderboard.Infrastructure.Exceptions;
using System.Text.Json;

namespace Leaderboard.Infrastructure.Errors;

internal sealed class ErrorHandlerMiddleware : IMiddleware
{
    private readonly ILogger<ErrorHandlerMiddleware> _logger;
    private readonly IExceptionToMessageMapper _exceptionToMessageMapper;

    public ErrorHandlerMiddleware(ILogger<ErrorHandlerMiddleware> logger, IExceptionToMessageMapper exceptionToMessageMapper)
    {
        _logger = logger;
        _exceptionToMessageMapper = exceptionToMessageMapper;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, exception.Message);
            await HandleErrorAsync(context, exception);
        }
    }

    private async Task HandleErrorAsync(HttpContext context, Exception exception)
    {
        // null for the optional message parameter
        var message = _exceptionToMessageMapper.Map(exception, null);
        HttpStatusCode code = exception switch
        {
            Leaderboard.Application.Exceptions.LeaderboardAppException => HttpStatusCode.BadRequest,
            Leaderboard.Core.Exceptions.LeaderboardException => HttpStatusCode.BadRequest,
            _ => HttpStatusCode.InternalServerError
        };

        context.Response.StatusCode = (int)code;
        context.Response.ContentType = "application/json";
        var response = message ?? new { code = "error", message = "An unexpected error occurred." };
        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}
