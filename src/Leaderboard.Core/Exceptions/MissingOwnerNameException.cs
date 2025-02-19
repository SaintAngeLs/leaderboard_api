namespace Leaderboard.Core.Exceptions;

public sealed class MissingOwnerNameException : LeaderboardException
{
    public MissingOwnerNameException()
        : base("Owner name must be provided.")
    {
    }
}
