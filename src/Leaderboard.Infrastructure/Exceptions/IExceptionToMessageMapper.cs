namespace Leaderboard.Infrastructure.Exceptions;

public interface IExceptionToMessageMapper
{
    object Map(Exception exception, object message);
}   