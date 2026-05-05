namespace Application.DTOs;

public record RegisterUserRequest(
    string UserName,
    string FirstName,
    string? SurName,
    string Email,
    string Password,
    string? Description,
    DateOnly? Birthday
);