using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Leaderboard.Core.Entities;
using Leaderboard.Core.Repositories;
using StackExchange.Redis;

namespace Leaderboard.Infrastructure.Redis.Repositories;

public class RedisCounterRepository : ICounterRepository
{
    private readonly IDatabase _database;
    private const string CounterKeyPrefix = "counter:"; 
    private const string CountersSetKey = "counters";

    public RedisCounterRepository(IConnectionMultiplexer connectionMultiplexer)
    {
        _database = connectionMultiplexer.GetDatabase();
    }

    public async Task AddAsync(Counter counter)
    {
        var key = CounterKeyPrefix + counter.Id;
        var json = JsonSerializer.Serialize(counter);
        await _database.StringSetAsync(key, json);
        await _database.SetAddAsync(CountersSetKey, counter.Id.ToString());
    }

    public async Task DeleteAsync(Guid id)
    {
        var key = CounterKeyPrefix + id;
        await _database.KeyDeleteAsync(key);
        await _database.SetRemoveAsync(CountersSetKey, id.ToString());
    }

    public async Task<Counter?> GetAsync(Guid id)
    {
        var key = CounterKeyPrefix + id;
        var json = await _database.StringGetAsync(key);
        if (json.IsNullOrEmpty) return null;
        return JsonSerializer.Deserialize<Counter>(json);
    }

    public async Task<IEnumerable<Counter>> GetAllAsync()
    {
        var counterIds = await _database.SetMembersAsync(CountersSetKey);
        var counters = new List<Counter>();
        foreach (var member in counterIds)
        {
            if (Guid.TryParse(member, out Guid id))
            {
                var counter = await GetAsync(id);
                if (counter != null)
                    counters.Add(counter);
            }
        }
        return counters;
    }

    public async Task UpdateAsync(Counter counter)
    {
        var key = CounterKeyPrefix + counter.Id;
        var json = JsonSerializer.Serialize(counter);
        await _database.StringSetAsync(key, json);
    }

    public async Task<IEnumerable<Counter>> GetCountersByTeamIdAsync(string teamId)
    {
        var allCounters = await GetAllAsync();
        if (Guid.TryParse(teamId, out Guid teamGuid))
        {
            return allCounters.Where(c => c.TeamId == teamGuid);
        }
        return Enumerable.Empty<Counter>();
    }
}
