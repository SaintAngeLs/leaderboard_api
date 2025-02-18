using Leaderboard.Core.Entities;

namespace Leaderboard.Application.Services.TeamService;

public interface ITeamService
{
    Task<Team> CreateTeamAsync(string teamName);
    Task<IEnumerable<Team>> GetAllTeamsAsync();
    Task<Team?> GetTeamAsync(Guid teamId);
    Task DeleteTeamAsync(Guid teamId);
}

