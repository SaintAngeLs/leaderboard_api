using Leaderboard.Application.Events;
using Leaderboard.Application.Exceptions;
using Leaderboard.Core.Entities;
using Leaderboard.Core.Repositories;
using Leaderboard.Core.ValueObjects;

namespace Leaderboard.Application.Services.TeamService;

public class TeamService : ITeamService
{
    private readonly ITeamRepository _teamRepository;
    private readonly IMessageBroker _messageBroker;

    public TeamService(ITeamRepository teamRepository, IMessageBroker messageBroker)
    {
        _teamRepository = teamRepository;
        _messageBroker = messageBroker;
    }

    public async Task<Team> CreateTeamAsync(string teamName)
    {
        var team = new Team(new TeamName(teamName));
        await _teamRepository.AddAsync(team);
        await _messageBroker.PublishAsync(new TeamCreated(team.Id, team.Name.ToString()));
        return team;
    }

    public async Task<IEnumerable<Team>> GetAllTeamsAsync()
    {
        return await _teamRepository.GetAllAsync();
    }

    public async Task<Team?> GetTeamAsync(Guid teamId)
    {
        return await _teamRepository.GetAsync(teamId);
    }

    public async Task DeleteTeamAsync(Guid teamId)
    {
        var team = await _teamRepository.GetAsync(teamId);
        if (team == null)
        {
            throw new TeamNotFoundException(teamId);
        }
        await _teamRepository.DeleteAsync(teamId);
        await _messageBroker.PublishAsync(new TeamDeleted(teamId));
    }
}