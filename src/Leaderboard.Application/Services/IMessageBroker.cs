namespace Leaderboard.Application.Services;

public interface IMessageBroker
{
     Task PublishAsync<T>(T @event);
}