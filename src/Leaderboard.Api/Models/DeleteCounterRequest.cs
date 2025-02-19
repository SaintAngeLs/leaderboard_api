namespace Leaderboard.Api.Models;

public class DeleteCounterRequest
{
    public Guid TeamId { get; set; }
    public Guid CounterId { get; set; }
}
