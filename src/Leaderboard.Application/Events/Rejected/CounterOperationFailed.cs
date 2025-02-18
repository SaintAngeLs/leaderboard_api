namespace Leaderboard.Application.Events.Rejected;

public class CounterOperationFailed
{
    public Guid CounterId { get; }
    public string Reason { get; }
    public string Code { get; }

    public CounterOperationFailed(Guid counterId, string reason, string code)
    {
        CounterId = counterId;
        Reason = reason;
        Code = code;
    }
}
