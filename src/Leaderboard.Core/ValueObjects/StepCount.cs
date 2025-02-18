using Leaderboard.Core.Exceptions;

namespace Leaderboard.Core.ValueObjects;

public class StepCount : IEquatable<StepCount>
{
    public int Value { get; }

    public StepCount(int value)
    {
        if (value < 0)
            throw new NegativeStepCountException(value);

        Value = value;
    }

    public StepCount Increment(int steps)
    {
        if (steps <= 0)
            throw new InvalidStepIncrementException(steps);

        return new StepCount(Value + steps);
    }

    public bool Equals(StepCount other)
    {
        if (other is null)
            return false;

        return Value == other.Value;
    }

    public override bool Equals(object obj) => Equals(obj as StepCount);

    public override int GetHashCode() => Value.GetHashCode();

    public override string ToString() => Value.ToString();
}