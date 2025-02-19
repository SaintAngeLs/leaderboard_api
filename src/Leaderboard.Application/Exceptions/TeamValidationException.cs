namespace Leaderboard.Application.Exceptions;

public class TeamValidationException : LeaderboardAppException
{
    public override string Code { get; } = "team_validation_error";

    public TeamValidationException(string message)
        : base(message)
    {
    }
}

