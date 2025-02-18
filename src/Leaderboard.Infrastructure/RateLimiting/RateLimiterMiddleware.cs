using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;

namespace Leaderboard.Infrastructure.RateLimiting;

internal sealed class RateLimiterMiddleware : IMiddleware
{
    private readonly IMemoryCache _cache;
    private readonly int _maxRequests = 100; // Maximum requests per time window
    private readonly TimeSpan _timeWindow = TimeSpan.FromMinutes(1);

    public RateLimiterMiddleware(IMemoryCache cache)
    {
        _cache = cache;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var clientIp = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        var cacheKey = $"RateLimiter_{clientIp}";

        int requestCount = _cache.GetOrCreate(cacheKey, entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = _timeWindow;
            return 0;
        });

        requestCount++;
        _cache.Set(cacheKey, requestCount, _timeWindow);

        if (requestCount > _maxRequests)
        {
            context.Response.StatusCode = (int)HttpStatusCode.TooManyRequests;
            await context.Response.WriteAsync("Rate limit exceeded. Please try again later.");
            return;
        }

        await next(context);
    }
}

