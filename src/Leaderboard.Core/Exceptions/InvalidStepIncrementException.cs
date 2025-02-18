namespace Leaderboard.Core.Exceptions;

public sealed class InvalidStepIncrementException : LeaderboardException
{
    public InvalidStepIncrementException(int increment)
        : base($"Step increment '{increment}' is invalid. Must be positive.")
    {
    }
}
