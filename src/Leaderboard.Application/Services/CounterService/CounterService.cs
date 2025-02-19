using Leaderboard.Application.Commands;
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

    public async Task AddCounterAsync(AddCounter command)
    {
        var team = await _teamRepository.GetAsync(command.TeamId);
        if (team == null)
        {
            throw new TeamNotFoundException(command.TeamId);
        }

        var counter = new Counter(command.OwnerName, command.TeamId);
        team.AddCounter(counter);
        await _teamRepository.UpdateAsync(team);
        await _counterRepository.AddAsync(counter);
        await _messageBroker.PublishAsync(new CounterAdded(command.TeamId, counter.Id, command.OwnerName));
    }

    public async Task IncrementCounterAsync(IncrementCounter command)
    {
        var team = await _teamRepository.GetAsync(command.TeamId);
        if (team == null)
        {
            throw new TeamNotFoundException(command.TeamId);
        }

        var counter = team.Counters.FirstOrDefault(c => c.Id == command.CounterId);
        if (counter == null)
        {
            throw new CounterNotFoundException(command.CounterId);
        }

        counter.Increment(command.Steps);
        await _teamRepository.UpdateAsync(team);
        await _counterRepository.UpdateAsync(counter);
        await _messageBroker.PublishAsync(new CounterIncremented(command.TeamId, command.CounterId, command.Steps));
    }

    public async Task DeleteCounterAsync(DeleteCounter command)
    {
        var team = await _teamRepository.GetAsync(command.TeamId);
        if (team == null)
        {
            throw new TeamNotFoundException(command.TeamId);
        }

        var counter = team.Counters.FirstOrDefault(c => c.Id == command.CounterId);
        if (counter == null)
        {
            throw new CounterNotFoundException(command.CounterId);
        }

        team.Counters.Remove(counter);
        await _teamRepository.UpdateAsync(team);
        await _counterRepository.DeleteAsync(command.CounterId);
        await _messageBroker.PublishAsync(new CounterDeleted(command.TeamId, command.CounterId));
    }
}