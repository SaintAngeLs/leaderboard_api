using Leaderboard.Application.DTO;
using Leaderboard.Application.Queries;
using Leaderboard.Core.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Leaderboard.Infrastructure.Queries;

public class GetAllTeamsQueryHandler : IQueryHandler<Leaderboard.Application.Queries.GetAllTeamsQuery, PaginatedResponse<TeamDto>>
{
    private readonly ITeamRepository _teamRepository;
    private readonly ICounterRepository _counterRepository;

    public GetAllTeamsQueryHandler(ITeamRepository teamRepository, ICounterRepository counterRepository)
    {
        _teamRepository = teamRepository;
        _counterRepository = counterRepository;
    }

    public async Task<PaginatedResponse<TeamDto>> HandleAsync(Leaderboard.Application.Queries.GetAllTeamsQuery query)
    {
        var teams = await _teamRepository.GetAllTeamsAsync();
        var totalCount = teams.Count();

        var pagedTeams = teams
            .Skip((query.PageNumber - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToList();

        var teamDtos = new List<TeamDto>();

        foreach (var team in pagedTeams)
        {
            var counters = await _counterRepository.GetCountersByTeamIdAsync(team.Id.ToString());
            int totalSteps = counters.Sum(c => c.Steps.Value);

            teamDtos.Add(new TeamDto
            {
                Id = team.Id,
                TeamName = team.Name.Value,
                TotalSteps = totalSteps
            });
        }

        return new PaginatedResponse<TeamDto>
        {
            Items = teamDtos,
            TotalCount = totalCount,
            PageNumber = query.PageNumber,
            PageSize = query.PageSize
        };
    }
}

