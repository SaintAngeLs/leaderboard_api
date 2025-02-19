namespace Leaderboard.Api.Models;

public class CreateCounterRequest
{
    public Guid TeamId { get; set; } = Guid.Empty;
    public string OwnerName { get; set; } = string.Empty;
}
