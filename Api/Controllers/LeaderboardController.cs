using Application.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LeaderboardController(ILeaderboardService leaderboardService) : ControllerBase
{
    [HttpGet("authors")]
    public async Task<ActionResult<LeaderboardEntry[]>> GetTopAuthors([FromQuery] int top = 10, CancellationToken cancellationToken = default)
    {
        if (top is < 1 or > 100)
        {
            return BadRequest("top must be between 1 and 100");
        }

        var result = await leaderboardService.GetTopAuthorsByPostsAsync(top, cancellationToken);
        return Ok(result);
    }
}

