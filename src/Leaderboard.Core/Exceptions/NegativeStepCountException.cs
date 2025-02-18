namespace Leaderboard.Core.Exceptions;

public sealed class NegativeStepCountException : LeaderboardException
{
    public NegativeStepCountException(int stepCount)
        : base($"Step count '{stepCount}' cannot be negative.")
    {
    }
}
