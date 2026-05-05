namespace Domain.Events;

public record PostCreatedDomainEvent(Guid PostId, Guid AuthorUserId, string Title, DateTime OccurredAt) : IDomainEvent;

