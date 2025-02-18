using System;

namespace Leaderboard.Core.Exceptions;

public sealed class MissingTeamIdException : LeaderboardException
{
    public MissingTeamIdException()
        : base("Team ID must be provided and cannot be empty.")
    {
    }
}

