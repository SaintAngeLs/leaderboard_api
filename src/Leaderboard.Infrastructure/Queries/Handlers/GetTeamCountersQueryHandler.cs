using Leaderboard.Application.DTO;
using Leaderboard.Application.Queries;
using Leaderboard.Core.Repositories;
using System.Linq;
using System.Threading.Tasks;

namespace Leaderboard.Infrastructure.Queries;

public class GetTeamCountersQueryHandler : IQueryHandler<Leaderboard.Application.Queries.GetTeamCountersQuery, PaginatedResponse<CounterDto>>
{
    private readonly ICounterRepository _counterRepository;

    public GetTeamCountersQueryHandler(ICounterRepository counterRepository)
    {
        _counterRepository = counterRepository;
    }

    public async Task<PaginatedResponse<CounterDto>> HandleAsync(Leaderboard.Application.Queries.GetTeamCountersQuery query)
    {
        var counters = await _counterRepository.GetCountersByTeamIdAsync(query.TeamId);
        var totalCount = counters.Count();

        var pagedItems = counters
            .Skip((query.PageNumber - 1) * query.PageSize)
            .Take(query.PageSize)
            .Select(c => new CounterDto
            {
                Id = c.Id,
                TeamId = c.TeamId, 
                OwnerName = c.OwnerName,
                Steps = c.Steps.Value
            })
            .ToList();

        return new PaginatedResponse<CounterDto>
        {
            Items = pagedItems,
            TotalCount = totalCount,
            PageNumber = query.PageNumber,
            PageSize = query.PageSize
        };
    }
}
