using Leaderboard.Core.Entities;

namespace Leaderboard.Core.Repositories;

public interface ITeamRepository
{
    Task<Team?> GetAsync(Guid id);
    Task<IEnumerable<Team>> GetAllAsync();
    Task AddAsync(Team team);
    Task DeleteAsync(Guid id);
    Task UpdateAsync(Team team);
    Task<IEnumerable<Team>> GetAllTeamsAsync();
}

