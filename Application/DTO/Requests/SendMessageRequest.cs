namespace Application.DTO.Requests;

public record SendMessageRequest(Guid ConversationId, string Content);
