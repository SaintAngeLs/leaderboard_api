using System.Collections.Generic;
using Leaderboard.Application.DTO;

namespace Leaderboard.Application.Queries;

public class GetAllTeamsQuery : IQuery<PaginatedResponse<TeamDto>>
{
    public int PageNumber { get; }
    public int PageSize { get; }

    public GetAllTeamsQuery(int pageNumber, int pageSize)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
    }
}