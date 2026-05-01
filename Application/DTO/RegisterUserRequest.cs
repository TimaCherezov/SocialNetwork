namespace Application.DTOs;

public class RegisterUserRequest
{
    public string UserName { get; set; } 
    public string FirstName { get; set; } 
    public string? SurName { get; set; } 
    public string Email { get; set; }
    public string Password { get; set; } 
    public string? Description { get; set; }
    public DateOnly? Birthday { get; set; }
}