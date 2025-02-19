using Leaderboard.Core.Exceptions;
using Leaderboard.Core.ValueObjects;

namespace Leaderboard.Core.Entities;

public class Counter
{
    public Guid Id { get; private set; }
    public string OwnerName { get; private set; }
    public StepCount Steps { get; private set; }

    public Counter(string ownerName)
    {
        if (string.IsNullOrWhiteSpace(ownerName))
            throw new MissingOwnerNameException();

        Id = Guid.NewGuid();
        OwnerName = ownerName;
        Steps = new StepCount(0);
    }

    public void Increment(int stepIncrement)
    {
        Steps = Steps.Increment(stepIncrement);
    }
}
