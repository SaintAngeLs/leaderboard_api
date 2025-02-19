using Leaderboard.Application.Services;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Leaderboard.Infrastructure.Services;

public class MessageBroker : IMessageBroker
{
    private readonly ILogger<MessageBroker> _logger;

    public MessageBroker(ILogger<MessageBroker> logger)
    {
        _logger = logger;
    }

    public Task PublishAsync<T>(T @event)
    {
        _logger.LogInformation("Publishing event of type {EventType} with data: {@EventData}",
            typeof(T).Name, @event);

        return Task.CompletedTask;
    }
}

