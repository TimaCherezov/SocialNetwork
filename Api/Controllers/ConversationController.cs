using Application.Abstractions;
using Application.DTO.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;


[ApiController]
[Route("api/[controller]")]
public class ConversationController(ICreateConversationService createConversationService) : ControllerBase
{
    [Authorize]
    [HttpPost("create")]
    public async Task<ActionResult<Guid>> CreatePrivateConversation(
        [FromBody] CreateConversationRequest request,
        CancellationToken ct)
    {
        var conversationId =
            await createConversationService
                .CreatePrivateConversationAsync(
                    request.UserBId,
                    ct);

        return Ok(conversationId);
    }
}