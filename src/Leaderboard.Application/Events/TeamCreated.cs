namespace Leaderboard.Application.Events;

public class TeamCreated
{
    public Guid TeamId { get; }
    public string TeamName { get; }
    public TeamCreated(Guid teamId, string teamName)
    {
        TeamId = teamId;
        TeamName = teamName;
    }
}
