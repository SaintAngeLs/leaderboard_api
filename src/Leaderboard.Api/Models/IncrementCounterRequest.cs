namespace Leaderboard.Api.Models;

public class IncrementCounterRequest
{
    public Guid TeamId { get; set; }
    public Guid CounterId { get; set; }
    public int Steps { get; set; }
}

