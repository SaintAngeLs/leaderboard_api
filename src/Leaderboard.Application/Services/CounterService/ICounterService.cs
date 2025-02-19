using Leaderboard.Application.Commands;

namespace Leaderboard.Application.Services.CounterService;

public interface ICounterService
{
    Task AddCounterAsync(AddCounter command);
    Task IncrementCounterAsync(IncrementCounter command);
    Task DeleteCounterAsync(DeleteCounter command);
}
