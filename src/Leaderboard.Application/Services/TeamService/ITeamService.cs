#nullable enable

using Leaderboard.Application.Commands;
using Leaderboard.Application.DTO;
using Leaderboard.Core.Entities;

namespace Leaderboard.Application.Services.TeamService;

public interface ITeamService
{
    Task<Team> CreateTeamAsync(CreateTeam command);
    Task DeleteTeamAsync(DeleteTeam command);
    Task<PaginatedResponse<TeamDto>> GetAllTeamsAsync(int pageNumber, int pageSize);
    Task<Team?> GetTeamAsync(Guid teamId);
}
