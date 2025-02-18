using System.Collections.Generic;
using Leaderboard.Application.DTO;

namespace Leaderboard.Application.Queries;

 public class GetTeamCountersQuery : IQuery<PaginatedResponse<CounterDto>>
{
    public string TeamId { get; }
    public int PageNumber { get; }
    public int PageSize { get; }

    public GetTeamCountersQuery(string teamId, int pageNumber, int pageSize)
    {
        TeamId = teamId;
        PageNumber = pageNumber;
        PageSize = pageSize;
    }
}