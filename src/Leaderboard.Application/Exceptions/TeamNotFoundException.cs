namespace Leaderboard.Application.Exceptions;

public class TeamNotFoundException : LeaderboardAppException
{
    public override string Code { get; } = "team_not_found";
    public Guid TeamId { get; }
    public TeamNotFoundException(Guid teamId)
        : base($"Team with id {teamId} not found.")
    {
        TeamId = teamId;
    }
}
