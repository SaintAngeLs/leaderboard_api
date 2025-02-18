namespace Leaderboard.Application.Commands;

public class DeleteTeam
{
    public Guid TeamId { get; }
    public DeleteTeam(Guid teamId)
    {
        TeamId = teamId;
    }
}

