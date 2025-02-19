using System;
using Leaderboard.Application.Events.Rejected;
using Leaderboard.Application.Exceptions;
using Leaderboard.Core.Exceptions;

namespace Leaderboard.Infrastructure.Exceptions;

internal sealed class ExceptionToMessageMapper : IExceptionToMessageMapper
{
    public object Map(Exception exception, object message)
        => exception switch
        {
            TeamNotFoundException ex 
                => new TeamOperationFailed(ex.TeamId, ex.Message, "team_not_found"),
            CounterNotFoundException ex 
                => new CounterOperationFailed(ex.CounterId, ex.Message, "counter_not_found"),
            _ => null        
        };
}

