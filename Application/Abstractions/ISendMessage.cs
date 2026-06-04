using Application.DTO;

namespace Application.Abstractions;

public interface ISendMessage
{
    Task<MessageDto> SendAsync(Guid conversationId, string content, CancellationToken ct = default);

}