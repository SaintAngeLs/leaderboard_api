using Leaderboard.Application.DTO;

namespace Leaderboard.Application.Queries;

public class GetTeamByIdQuery : IQuery<TeamDto>
{
    public Guid TeamId { get; }

    public GetTeamByIdQuery(Guid teamId)
    {
        TeamId = teamId;
    }
}

