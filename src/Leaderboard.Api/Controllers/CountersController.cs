namespace Leaderboard.Api.Controllers;

using Leaderboard.Api.Models;
using Leaderboard.Application.Commands;
using Leaderboard.Application.DTO;
using Leaderboard.Application.Exceptions;
using Leaderboard.Application.Queries;
using Leaderboard.Application.Services.CounterService;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

/// <summary>
/// Controller for managing counters.
/// </summary>
[ApiController]
[Route("api/counters")]
public class CountersController : ControllerBase
{
    private readonly ICounterService _counterService;
    private readonly IQueryHandler<GetTeamCountersQuery, PaginatedResponse<CounterDto>> _getCountersHandler;
    private readonly IQueryHandler<GetTeamTotalStepsQuery, int> _getTotalStepsHandler;

    /// <summary>
    /// Initializes a new instance of the <see cref="CountersController"/> class.
    /// </summary>
    public CountersController(
        ICounterService counterService,
        IQueryHandler<GetTeamCountersQuery, PaginatedResponse<CounterDto>> getCountersHandler,
        IQueryHandler<GetTeamTotalStepsQuery, int> getTotalStepsHandler)
    {
        _counterService = counterService;
        _getCountersHandler = getCountersHandler;
        _getTotalStepsHandler = getTotalStepsHandler;
    }

    /// <summary>
    /// Creates a new counter for a given team.
    /// </summary>
    /// <param name="request">The request containing team ID and owner name.</param>
    /// <returns>A confirmation message if successful.</returns>
    /// <response code="200">Counter created successfully.</response>
    /// <response code="404">Team not found.</response>
    [HttpPost("create")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddCounter([FromBody] CreateCounterRequest request)
    {
        try
        {
            var command = new AddCounter(request.TeamId, request.OwnerName);
            await _counterService.AddCounterAsync(command);
            return Ok("Counter created successfully.");
        }
        catch (TeamNotFoundException)
        {
            return NotFound($"Team with ID {request.TeamId} was not found.");
        }
    }

    /// <summary>
    /// Increments a counter's step count.
    /// </summary>
    /// <param name="request">The request containing team ID, counter ID, and the number of steps to increment.</param>
    /// <returns>A confirmation message if successful.</returns>
    /// <response code="200">Counter incremented successfully.</response>
    /// <response code="400">Bad request if input is invalid.</response>
    /// <response code="404">Team or counter not found.</response>
    [HttpPut("increment")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)] 
    [ProducesResponseType(StatusCodes.Status404NotFound)] 
    public async Task<IActionResult> IncrementCounter([FromBody] IncrementCounterRequest request)
    {
        try
        {
            var command = new IncrementCounter(request.TeamId, request.CounterId, request.Steps);
            await _counterService.IncrementCounterAsync(command);
            return Ok("Counter incremented successfully.");
        }
        catch (TeamNotFoundException)
        {
            return NotFound($"Team with ID {request.TeamId} was not found.");
        }
        catch (CounterNotFoundException)
        {
            return NotFound($"Counter with ID {request.CounterId} was not found.");
        }
    }

    /// <summary>
    /// Deletes a counter.
    /// </summary>
    /// <param name="request">The request containing team ID and counter ID.</param>
    /// <returns>A confirmation message if successful.</returns>
    /// <response code="200">Counter deleted successfully.</response>
    /// <response code="404">Team or counter not found.</response>
    [HttpDelete("delete")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteCounter([FromBody] DeleteCounterRequest request)
    {
        try
        {
            var command = new DeleteCounter(request.TeamId, request.CounterId);
            await _counterService.DeleteCounterAsync(command);
            return Ok("Counter deleted successfully.");
        }
        catch (TeamNotFoundException)
        {
            return NotFound($"Team with ID {request.TeamId} was not found.");
        }
        catch (CounterNotFoundException)
        {
            return NotFound($"Counter with ID {request.CounterId} was not found.");
        }
    }

    
    /// <summary>
    /// Retrieves paginated counters for a given team.
    /// </summary>
    /// <param name="teamId">The team ID.</param>
    /// <param name="pageNumber">The page number (default is 1).</param>
    /// <param name="pageSize">The page size (default is 10).</param>
    /// <returns>A paginated response of counters.</returns>
    /// <response code="200">Returns the list of counters.</response>
    [HttpGet("team")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<PaginatedResponse<CounterDto>>> GetTeamCounters(
        [FromQuery] Guid teamId,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var query = new GetTeamCountersQuery(teamId.ToString(), pageNumber, pageSize);
        var result = await _getCountersHandler.HandleAsync(query);
        return Ok(result);
    }

    /// <summary>
    /// Retrieves the total steps for all counters of a given team.
    /// </summary>
    /// <param name="teamId">The team ID.</param>
    /// <returns>The total steps count.</returns>
    /// <response code="200">Returns the total steps.</response>
    [HttpGet("team/{teamId:guid}/totalsteps")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<int>> GetTeamTotalSteps(Guid teamId)
    {
        var query = new GetTeamTotalStepsQuery(teamId.ToString());
        int totalSteps = await _getTotalStepsHandler.HandleAsync(query);
        return Ok(totalSteps);
    }
}
