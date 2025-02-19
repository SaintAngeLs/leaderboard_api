using Leaderboard.Application.Queries;
using Leaderboard.Core.Repositories;
using System.Linq;
using System.Threading.Tasks;

namespace Leaderboard.Infrastructure.Queries;

public class GetTeamTotalStepsQueryHandler : IQueryHandler<Leaderboard.Application.Queries.GetTeamTotalStepsQuery, int>
{
    private readonly ICounterRepository _counterRepository;

    public GetTeamTotalStepsQueryHandler(ICounterRepository counterRepository)
    {
        _counterRepository = counterRepository;
    }

    public async Task<int> HandleAsync(Leaderboard.Application.Queries.GetTeamTotalStepsQuery query)
    {
        var counters = await _counterRepository.GetCountersByTeamIdAsync(query.TeamId);

        return counters?.Sum(c => c.Steps.Value) ?? 0;
    }
}

