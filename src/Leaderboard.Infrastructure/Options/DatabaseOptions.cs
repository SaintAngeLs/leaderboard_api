namespace Leaderboard.Infrastructure.Options;

public class DatabaseOptions
{
    // e.g., "InMemory", "PostgreSQL"
    public string Provider { get; set; } = "InMemory";
    public string ConnectionString { get; set; } = string.Empty;
}
