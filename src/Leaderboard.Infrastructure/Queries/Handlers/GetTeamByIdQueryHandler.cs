using System.Linq;
using System.Threading.Tasks;
using Leaderboard.Application.DTO;
using Leaderboard.Application.Exceptions;
using Leaderboard.Application.Queries;
using Leaderboard.Core.Repositories;

namespace Leaderboard.Infrastructure.Queries;

public class GetTeamByIdQueryHandler : IQueryHandler<GetTeamByIdQuery, TeamDto>
{
    private readonly ITeamRepository _teamRepository;
    private readonly ICounterRepository _counterRepository;

    public GetTeamByIdQueryHandler(ITeamRepository teamRepository, ICounterRepository counterRepository)
    {
        _teamRepository = teamRepository;
        _counterRepository = counterRepository;
    }

    public async Task<TeamDto> HandleAsync(GetTeamByIdQuery query)
    {
        // Retrieve the team from the repository
        var team = await _teamRepository.GetAsync(query.TeamId);
        if (team == null)
        {
            throw new TeamNotFoundException(query.TeamId);
        }

        // Retrieve counters for the team to compute the total steps.
        // Note: If your team entity already includes the counters (with lazy/eager loading),
        // you could use team.Counters instead.
        var counters = await _counterRepository.GetCountersByTeamIdAsync(query.TeamId.ToString());
        int totalSteps = counters.Sum(c => c.Steps.Value);

        return new TeamDto
        {
            Id = team.Id,
            TeamName = team.Name.Value,
            TotalSteps = totalSteps
        };
    }
}

