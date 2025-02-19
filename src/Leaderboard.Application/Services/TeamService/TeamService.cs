namespace Leaderboard.Application.Services.TeamService;

using Leaderboard.Application.Commands;
using Leaderboard.Application.DTO;
using Leaderboard.Application.Events;
using Leaderboard.Application.Exceptions;
using Leaderboard.Core.Entities;
using Leaderboard.Core.Repositories;
using Leaderboard.Core.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class TeamService : ITeamService
{
    private readonly ITeamRepository _teamRepository;
    private readonly IMessageBroker _messageBroker;

    public TeamService(ITeamRepository teamRepository, IMessageBroker messageBroker)
    {
        _teamRepository = teamRepository;
        _messageBroker = messageBroker;
    }

    public async Task<Team> CreateTeamAsync(CreateTeam command)
    {
        if (string.IsNullOrWhiteSpace(command.TeamName))
        {
            throw new TeamValidationException("Team name cannot be empty.");
        }

        var team = new Team(new TeamName(command.TeamName));
        await _teamRepository.AddAsync(team);
        await _messageBroker.PublishAsync(new TeamCreated(team.Id, team.Name.Value));
        return team;
    }

    public async Task DeleteTeamAsync(DeleteTeam command)
    {
        var team = await _teamRepository.GetAsync(command.TeamId);
        if (team == null)
        {
            throw new TeamNotFoundException(command.TeamId);
        }
        await _teamRepository.DeleteAsync(command.TeamId);
        await _messageBroker.PublishAsync(new TeamDeleted(command.TeamId));
    }

    public async Task<PaginatedResponse<TeamDto>> GetAllTeamsAsync(int pageNumber, int pageSize)
    {
        var teams = await _teamRepository.GetAllTeamsAsync();
        var totalCount = teams.Count();

        var pagedTeams = teams
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var teamDtos = pagedTeams.Select(team => new TeamDto
        {
            Id = team.Id,
            TeamName = team.Name.Value,
            TotalSteps = team.Counters?.Sum(c => c.Steps.Value) ?? 0
        }).ToList();

        return new PaginatedResponse<TeamDto>
        {
            Items = teamDtos,
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize
        };
    }

    public async Task<Team?> GetTeamAsync(Guid teamId)
    {
        return await _teamRepository.GetAsync(teamId);
    }
}
