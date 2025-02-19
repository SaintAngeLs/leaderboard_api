using Leaderboard.Application.Events;
using Leaderboard.Application.Exceptions;
using Leaderboard.Core.Entities;
using Leaderboard.Core.Repositories;

namespace Leaderboard.Application.Services.CounterService;

public class CounterService : ICounterService
{
    private readonly ITeamRepository _teamRepository;
    private readonly ICounterRepository _counterRepository;
    private readonly IMessageBroker _messageBroker;

    public CounterService(ITeamRepository teamRepository, ICounterRepository counterRepository, IMessageBroker messageBroker)
    {
        _teamRepository = teamRepository;
        _counterRepository = counterRepository;
        _messageBroker = messageBroker;
    }

    public async Task AddCounterToTeamAsync(Guid teamId, string ownerName)
    {
        var team = await _teamRepository.GetAsync(teamId);
        if (team == null)
        {
            throw new TeamNotFoundException(teamId);
        }

        var counter = new Counter(ownerName);
        team.AddCounter(counter);
        await _teamRepository.UpdateAsync(team);
        await _counterRepository.AddAsync(counter);
        await _messageBroker.PublishAsync(new CounterAdded(teamId, counter.Id, ownerName));
    }

    public async Task IncrementCounterAsync(Guid teamId, Guid counterId, int steps)
    {
        var team = await _teamRepository.GetAsync(teamId);
        if (team == null)
        {
            throw new TeamNotFoundException(teamId);
        }

        var counter = team.Counters.FirstOrDefault(c => c.Id == counterId);
        if (counter == null)
        {
            throw new CounterNotFoundException(counterId);
        }

        counter.Increment(steps);
        await _teamRepository.UpdateAsync(team);
        await _counterRepository.UpdateAsync(counter);
        await _messageBroker.PublishAsync(new CounterIncremented(teamId, counterId, steps));
    }

    public async Task DeleteCounterAsync(Guid teamId, Guid counterId)
    {
        var team = await _teamRepository.GetAsync(teamId);
        if (team == null)
        {
            throw new TeamNotFoundException(teamId);
        }

        var counter = team.Counters.FirstOrDefault(c => c.Id == counterId);
        if (counter == null)
        {
            throw new CounterNotFoundException(counterId);
        }

        team.Counters.Remove(counter);
        await _teamRepository.UpdateAsync(team);
        await _counterRepository.DeleteAsync(counterId);
        await _messageBroker.PublishAsync(new CounterDeleted(teamId, counterId));
    }
}