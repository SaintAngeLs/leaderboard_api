using Leaderboard.Api.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Leaderboard.Api.Controllers
{
    /// <summary>
    /// Provides endpoints for basic API functionality and error demonstration.
    /// </summary>
    [Route("api")]
    public class HomeController : ControllerBase
    {
        private readonly string _apiName;

        /// <summary>
        /// Initializes a new instance of the <see cref="HomeController"/> class.
        /// </summary>
        /// <param name="options">The API options containing configuration values.</param>
        public HomeController(IOptions<ApiOptions> options)
        {
            _apiName = options.Value.Name;
        }

        /// <summary>
        /// Retrieves the API name.
        /// </summary>
        /// <returns>A string representing the API name.</returns>
        [HttpGet("hello")]
        public ActionResult<string> Get() => _apiName;

        /// <summary>
        /// Returns a demonstration error response.
        /// </summary>
        /// <returns>
        /// A 500 Internal Server Error response containing an error code and message.
        /// </returns>
        [HttpGet("error")]
        public IActionResult Error()
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new 
            { 
                code = "error", 
                message = "oops, it's seems the error appeared!" 
            });
        }
    }
}
