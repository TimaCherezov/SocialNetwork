namespace Application.DTO;

public record PostResponse(Guid Id, string Title, string Content, DateTime CreatedAt);