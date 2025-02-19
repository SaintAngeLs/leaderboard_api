using Leaderboard.Core.Exceptions;

namespace Leaderboard.Core.ValueObjects;

public class TeamName : IEquatable<TeamName>
{
    public string Value { get; }

    public TeamName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new InvalidTeamNameException(value);

        Value = value.Trim();
    }

    public bool Equals(TeamName other)
    {
        if (other is null)
            return false;

        return string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);
    }

    public override bool Equals(object obj) => Equals(obj as TeamName);

    public override int GetHashCode() =>
        Value.GetHashCode(StringComparison.OrdinalIgnoreCase);

    public override string ToString() => Value;

    public static implicit operator string(TeamName teamName) => teamName.Value;
}

