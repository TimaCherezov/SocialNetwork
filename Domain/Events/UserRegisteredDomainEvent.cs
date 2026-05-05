namespace Domain.Events;

public record UserRegisteredDomainEvent(Guid UserId, string UserName, string Email, DateTime OccurredAt) : IDomainEvent;

