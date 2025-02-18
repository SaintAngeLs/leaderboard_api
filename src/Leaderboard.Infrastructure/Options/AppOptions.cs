namespace Leaderboard.Infrastructure.Options;

public class AppOptions
{
    public string AppName { get; set; } = "App";
    public DatabaseOptions Database { get; set; } = new DatabaseOptions();
}