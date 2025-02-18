namespace Leaderboard.Application.Commands;

public class IncrementCounter
{
    public Guid TeamId { get; }
    public Guid CounterId { get; }
    public int Steps { get; }
    public IncrementCounter(Guid teamId, Guid counterId, int steps)
    {
        TeamId = teamId;
        CounterId = counterId;
        Steps = steps;
    }
}

