namespace Leaderboard.Application.Events;

public class CounterDeleted
{
    public Guid TeamId { get; }
    public Guid CounterId { get; }
    public CounterDeleted(Guid teamId, Guid counterId)
    {
        TeamId = teamId;
        CounterId = counterId;
    }
}
