using Leaderboard.Core.Entities;

namespace Leaderboard.Core.Repositories;

public interface ICounterRepository
{
    Task<Counter?> GetAsync(Guid id);
    Task<IEnumerable<Counter>> GetAllAsync();
    Task AddAsync(Counter counter);
    Task DeleteAsync(Guid id);
    Task UpdateAsync(Counter counter);
}

