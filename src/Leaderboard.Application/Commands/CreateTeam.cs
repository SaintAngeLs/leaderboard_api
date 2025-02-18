namespace Leaderboard.Application.Commands;

public class CreateTeam
{
    public string TeamName { get; }
    public CreateTeam(string teamName)
    {
        TeamName = teamName;
    }
}

