namespace Leaderboard.Api.Controllers;

using Leaderboard.Api.Models;
using Leaderboard.Application.Commands;
using Leaderboard.Application.DTO;
using Leaderboard.Application.Exceptions;
using Leaderboard.Application.Queries;
using Leaderboard.Application.Services.TeamService;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

/// <summary>
/// Controller for managing teams.
/// </summary>
[ApiController]
[Route("api/teams")]
public class TeamsController : ControllerBase
{
    private readonly IQueryHandler<GetAllTeamsQuery, PaginatedResponse<TeamDto>> _allTeamsHandler;
    private readonly IQueryHandler<GetTeamByIdQuery, TeamDto> _getTeamByIdHandler;
    private readonly ITeamService _teamService;

    /// <summary>
    /// Initializes a new instance of the <see cref="TeamsController"/> class.
    /// </summary>
    /// <param name="allTeamsHandler">Query handler for retrieving all teams.</param>
    /// <param name="getTeamByIdHandler">Query handler for retrieving a team by its ID.</param>
    /// <param name="teamService">Service for team-related operations.</param>
    public TeamsController(
        IQueryHandler<GetAllTeamsQuery, PaginatedResponse<TeamDto>> allTeamsHandler,
        IQueryHandler<GetTeamByIdQuery, TeamDto> getTeamByIdHandler,
        ITeamService teamService)
    {
        _allTeamsHandler = allTeamsHandler;
        _getTeamByIdHandler = getTeamByIdHandler;
        _teamService = teamService;
    }

    /// <summary>
    /// Retrieves a paginated list of teams.
    /// </summary>
    /// <param name="pageNumber">The page number (default is 1).</param>
    /// <param name="pageSize">The page size (default is 10).</param>
    /// <returns>A paginated response containing teams.</returns>
    /// <response code="200">Returns the paginated list of teams.</response>
    [HttpGet]
    public async Task<ActionResult<PaginatedResponse<TeamDto>>> GetTeams([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var query = new GetAllTeamsQuery(pageNumber, pageSize);
        var result = await _allTeamsHandler.HandleAsync(query);
        return Ok(result);
    }

    /// <summary>
    /// Retrieves a team by its ID.
    /// </summary>
    /// <param name="id">The team ID.</param>
    /// <returns>The team details.</returns>
    /// <response code="200">Returns the team details.</response>
    /// <response code="404">If the team is not found.</response>
    [HttpGet("{id:guid}", Name = "GetTeamById")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TeamDto>> GetTeam(Guid id)
    {
        try
        {
            var query = new GetTeamByIdQuery(id);
            var teamDto = await _getTeamByIdHandler.HandleAsync(query);
            return Ok(teamDto);
        }
        catch (TeamNotFoundException ex)
        {
            return NotFound(new { code = ex.Code, message = ex.Message });
        }
    }

    /// <summary>
    /// Creates a new team.
    /// </summary>
    /// <param name="request">The team creation request containing the team name.</param>
    /// <returns>A newly created team.</returns>
    /// <response code="201">Returns the newly created team.</response>
    /// <response code="400">If the team name is invalid or empty.</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> CreateTeam([FromBody] CreateTeamRequest request)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.TeamName))
            {
                return BadRequest(new { code = "invalid_input", message = "Team name cannot be empty." });
            }

            var command = new CreateTeam(request.TeamName);
            var team = await _teamService.CreateTeamAsync(command);

            var teamDto = new TeamDto
            {
                Id = team.Id,
                TeamName = team.Name.Value,
                TotalSteps = team.Counters?.Sum(c => c.Steps.Value) ?? 0
            };

            return CreatedAtRoute("GetTeamById", new { id = team.Id }, teamDto);
        }
        catch (TeamValidationException ex)
        {
            return BadRequest(new { code = ex.Code, message = ex.Message });
        }
    }

    /// <summary>
    /// Deletes a team.
    /// </summary>
    /// <param name="request">The team deletion request containing the team ID.</param>
    /// <returns>A confirmation message.</returns>
    /// <response code="200">If the team was deleted successfully.</response>
    /// <response code="404">If the team is not found.</response>
    [HttpDelete("delete")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeleteTeam([FromBody] DeleteTeamRequest request)
    {
        try
        {
            var command = new DeleteTeam(request.TeamId);
            await _teamService.DeleteTeamAsync(command);
            return Ok("Team deleted successfully.");
        }
        catch (TeamNotFoundException ex)
        {
            return NotFound(new { code = ex.Code, message = ex.Message });
        }
    }
}
