namespace Leaderboard.Application.Events.Rejected;

public class TeamOperationFailed
{
    public Guid TeamId { get; }
    public string Reason { get; }
    public string Code { get; }

    public TeamOperationFailed(Guid teamId, string reason, string code)
    {
        TeamId = teamId;
        Reason = reason;
        Code = code;
    }
}
