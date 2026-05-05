using Application.Abstractions;
using Application.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PostController(ICreatePostService createPostService) : ControllerBase
{
    [Authorize]
    [HttpPost("create")]
    public async Task<IActionResult> CreatePost([FromBody] CreatePostRequest request, CancellationToken cancellationToken)
    {
        var response = await createPostService.CreatePost(request, cancellationToken);
        return Ok(response);
    }
}