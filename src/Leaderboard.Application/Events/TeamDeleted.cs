namespace Leaderboard.Application.Events;

public class TeamDeleted
{
    public Guid TeamId { get; }
    public TeamDeleted(Guid teamId)
    {
        TeamId = teamId;
    }
}

