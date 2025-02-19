using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Leaderboard.Infrastructure.RateLimiting;

public static class Extensions
{
    public static IServiceCollection AddRateLimiting(this IServiceCollection services)
        => services.AddScoped<RateLimiterMiddleware>();

    public static IApplicationBuilder UseRateLimiting(this IApplicationBuilder app)
        => app.UseMiddleware<RateLimiterMiddleware>();
}

