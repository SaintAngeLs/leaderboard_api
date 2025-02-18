using Leaderboard.Core.ValueObjects;

namespace Leaderboard.Core.Entities;

public class Team
{
    public Guid Id { get; private set; }
    public TeamName Name { get; private set; }
    public List<Counter> Counters { get; private set; }

    public Team(TeamName name)
    {
        Id = Guid.NewGuid();
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Counters = new List<Counter>();
    }

    public void AddCounter(Counter counter)
    {
        if (counter == null)
            throw new ArgumentNullException(nameof(counter));
        Counters.Add(counter);
    }
}

