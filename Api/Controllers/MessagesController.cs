using Application.Abstractions;
using Application.DTO;
using Application.DTO.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class MessagesController(ISendMessage sendMessage) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<MessageDto>> Send([FromBody] SendMessageRequest request, CancellationToken ct)
    {
        var dto = await sendMessage.SendAsync(request.ConversationId, request.Content, ct);
        return Ok(dto);
    }
}

