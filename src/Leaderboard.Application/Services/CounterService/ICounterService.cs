namespace Leaderboard.Application.Services.CounterService;

public interface ICounterService
{
    Task AddCounterToTeamAsync(Guid teamId, string ownerName);
    Task IncrementCounterAsync(Guid teamId, Guid counterId, int steps);
    Task DeleteCounterAsync(Guid teamId, Guid counterId);
}
