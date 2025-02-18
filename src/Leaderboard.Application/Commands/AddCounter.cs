namespace Leaderboard.Application.Commands;

public class AddCounter
{
    public Guid TeamId { get; }
    public string OwnerName { get; }
    public AddCounter(Guid teamId, string ownerName)
    {
        TeamId = teamId;
        OwnerName = ownerName;
    }
}

