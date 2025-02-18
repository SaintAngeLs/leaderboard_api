namespace Leaderboard.Application.Commands;

public class DeleteCounter
{
    public Guid TeamId { get; }
    public Guid CounterId { get; }
    public DeleteCounter(Guid teamId, Guid counterId)
    {
        TeamId = teamId;
        CounterId = counterId;
    }
}
