namespace Application.DTO;

public record MessageDto(Guid Id, Guid ConversationId, Guid SenderUserId, string Content, DateTime SentAt);

