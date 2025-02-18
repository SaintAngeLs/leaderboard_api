using System.Text.Json;
using Leaderboard.Core.Entities;
using Leaderboard.Core.Repositories;
using StackExchange.Redis;

namespace Leaderboard.Infrastructure.Redis.Repositories;

public class RedisTeamRepository : ITeamRepository
{
    private readonly IDatabase _database;
    private const string TeamKeyPrefix = "team:"; 
    private const string TeamsSetKey = "teams";

    public RedisTeamRepository(IConnectionMultiplexer connectionMultiplexer)
    {
        _database = connectionMultiplexer.GetDatabase();
    }

    public async Task AddAsync(Team team)
    {
        var key = TeamKeyPrefix + team.Id;
        var json = JsonSerializer.Serialize(team);
        await _database.StringSetAsync(key, json);
        await _database.SetAddAsync(TeamsSetKey, team.Id.ToString());
    }

    public async Task DeleteAsync(Guid id)
    {
        var key = TeamKeyPrefix + id;
        await _database.KeyDeleteAsync(key);
        await _database.SetRemoveAsync(TeamsSetKey, id.ToString());
    }

    public async Task<Team?> GetAsync(Guid id)
    {
        var key = TeamKeyPrefix + id;
        var json = await _database.StringGetAsync(key);
        if (json.IsNullOrEmpty) return null;
        return JsonSerializer.Deserialize<Team>(json);
    }

    public async Task<IEnumerable<Team>> GetAllAsync()
    {
        var teamIds = await _database.SetMembersAsync(TeamsSetKey);
        var teams = new List<Team>();
        foreach (var member in teamIds)
        {
            if (Guid.TryParse(member, out Guid id))
            {
                var team = await GetAsync(id);
                if (team != null)
                    teams.Add(team);
            }
        }
        return teams;
    }

    public async Task UpdateAsync(Team team)
    {
        var key = TeamKeyPrefix + team.Id;
        var json = JsonSerializer.Serialize(team);
        await _database.StringSetAsync(key, json);
    }
}

