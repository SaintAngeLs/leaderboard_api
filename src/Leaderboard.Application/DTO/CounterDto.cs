namespace Leaderboard.Application.DTO;

public class CounterDto
{
    public Guid Id { get; set; }
    public string OwnerName { get; set; }
    public int Steps { get; set; }
    public Guid TeamId { get; set; }
}
