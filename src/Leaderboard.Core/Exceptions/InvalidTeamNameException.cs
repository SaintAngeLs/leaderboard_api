namespace Leaderboard.Core.Exceptions;

public sealed class InvalidTeamNameException : LeaderboardException
{
    public InvalidTeamNameException(string teamName)
        : base($"Team name '{teamName}' is invalid!")
    {
    }
}
