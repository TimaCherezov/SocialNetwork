namespace Domain.Entities;

public class Post
{
    public Guid Id { get; private set; }
    public string Title { get; private set; }
    public string Content { get; private set; }
    public Guid UserId { get; private set; }
    public User User { get; private set; } 

}