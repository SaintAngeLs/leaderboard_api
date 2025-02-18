namespace Leaderboard.Application.Queries;

public class GetTeamTotalStepsQuery : IQuery<int>
{
    public string TeamId { get; }

    public GetTeamTotalStepsQuery(string teamId)
    {
        TeamId = teamId;
    }
}