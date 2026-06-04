namespace Domain.Events;

public record MessageSentDomainEvent(
	Guid ConversationId,
	Guid MessageId,
	Guid SenderUserId,
	Guid[] RecipientUserIds,
	string Content,
	DateTime SentAt) : IDomainEvent;
