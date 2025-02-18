namespace Leaderboard.Application.Exceptions;

public abstract class LeaderboardAppException : Exception
{
    public virtual string Code { get; }
    protected LeaderboardAppException(string message) : base(message) { }
}
