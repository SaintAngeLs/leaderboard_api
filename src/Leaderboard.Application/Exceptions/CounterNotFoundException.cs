namespace Leaderboard.Application.Exceptions;

public class CounterNotFoundException : LeaderboardAppException
{
    public override string Code { get; } = "counter_not_found";
    public Guid CounterId { get; }
    public CounterNotFoundException(Guid counterId)
        : base($"Counter with id {counterId} not found.")
    {
        CounterId = counterId;
    }
}

