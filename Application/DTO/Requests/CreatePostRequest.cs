namespace Application.DTO;

public record CreatePostRequest(string Title, string Content, Guid AuthorId);