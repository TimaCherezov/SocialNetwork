namespace Application.DTO.Responses;

public record NotificationResponse(Guid UserId, string Message, DateTime CreatedAt, bool IsRead);