namespace Leaderboard.Application.Events;

public class CounterAdded
{
    public Guid TeamId { get; }
    public Guid CounterId { get; }
    public string OwnerName { get; }
    public CounterAdded(Guid teamId, Guid counterId, string ownerName)
    {
        TeamId = teamId;
        CounterId = counterId;
        OwnerName = ownerName;
    }
}
