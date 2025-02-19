using Leaderboard.Application.Services.CounterService;
using Leaderboard.Application.Services.TeamService;
using Microsoft.Extensions.DependencyInjection;

namespace Leaderboard.Application;

public static class Extensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ICounterService, CounterService>();
        services.AddScoped<ITeamService, TeamService>();
        
        return services;
    }
}