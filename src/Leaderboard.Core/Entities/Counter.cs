using System.Text.Json.Serialization;
using Leaderboard.Core.Exceptions;
using Leaderboard.Core.ValueObjects;

namespace Leaderboard.Core.Entities;

public class Counter
{
    public Guid Id { get; private set; }
    public string OwnerName { get; private set; }
    public StepCount Steps { get; private set; }
    public Guid TeamId { get; private set; }

    public Counter(string ownerName, Guid teamId)
    {
        if (string.IsNullOrWhiteSpace(ownerName))
            throw new MissingOwnerNameException();
        
        if (teamId == Guid.Empty)
                throw new MissingTeamIdException();

        Id = Guid.NewGuid();
        OwnerName = ownerName;
        TeamId = teamId;
        Steps = new StepCount(0);
    }

    [JsonConstructor]
    public Counter(Guid id, string ownerName, StepCount steps, Guid teamId)
    {
        Id = id;
        OwnerName = ownerName;
        Steps = steps;
        TeamId = teamId;
    }


    public void Increment(int stepIncrement)
    {
        Steps = Steps.Increment(stepIncrement);
    }
}
