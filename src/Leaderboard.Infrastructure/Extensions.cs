using Leaderboard.Application.DTO;
using Leaderboard.Application.Queries;
using Leaderboard.Application.Services;
using Leaderboard.Core.Repositories;
using Leaderboard.Infrastructure.Auth;
using Leaderboard.Infrastructure.Errors;
using Leaderboard.Infrastructure.Exceptions;
using Leaderboard.Infrastructure.Options;
using Leaderboard.Infrastructure.Queries;
using Leaderboard.Infrastructure.RateLimiting;
using Leaderboard.Infrastructure.Redis.Repositories;
using Leaderboard.Infrastructure.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Leaderboard.Infrastructure;

public static class Extensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AppOptions>(options =>
        {
            configuration.GetSection("AppOptions").Bind(options);
        });

        var redisConnectionString = configuration.GetSection("AppOptions:Database:ConnectionString").Value ?? string.Empty;
        var redis = ConnectionMultiplexer.Connect(redisConnectionString);
        services.AddSingleton<IConnectionMultiplexer>(redis);

        services.AddScoped<ITeamRepository, RedisTeamRepository>();
        services.AddScoped<ICounterRepository, RedisCounterRepository>();

         services.AddSingleton<IMessageBroker, MessageBroker>();

        services.AddErrorHandling();
        services.AddScoped<IExceptionToMessageMapper, ExceptionToMessageMapper>();

        services.AddTransient<IQueryHandler<GetTeamTotalStepsQuery, int>, GetTeamTotalStepsQueryHandler>();
        services.AddTransient<IQueryHandler<GetTeamCountersQuery, PaginatedResponse<CounterDto>>, GetTeamCountersQueryHandler>();
        services.AddTransient<IQueryHandler<GetAllTeamsQuery, PaginatedResponse<TeamDto>>, GetAllTeamsQueryHandler>();

        services.AddAuth();
        services.AddRateLimiting();

        return services;
    }

    public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder app)
    {
        app.UseAuth();
        app.UseRateLimiting();
        app.UseErrorHandling();
        return app;
    }
}

