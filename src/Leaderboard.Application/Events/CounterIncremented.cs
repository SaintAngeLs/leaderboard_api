namespace Leaderboard.Application.Events;

public class CounterIncremented
{
    public Guid TeamId { get; }
    public Guid CounterId { get; }
    public int Steps { get; }
    public CounterIncremented(Guid teamId, Guid counterId, int steps)
    {
        TeamId = teamId;
        CounterId = counterId;
        Steps = steps;
    }
}
