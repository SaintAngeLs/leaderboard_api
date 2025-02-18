namespace Leaderboard.Core.Exceptions;

public abstract class LeaderboardException : Exception
{
    protected LeaderboardException(string message) : base(message)
    {
    }
}
