using Application.Abstractions;
using Application.DTO;
using Application.Mappers;
using Domain.Abstractions;
using Domain.Entities;
using Domain.Events;

namespace Application.Services;

public class CreatePostService(IUnitOfWork unitOfWork, IEventDispatcher eventDispatcher) : ICreatePostService
{
    public async Task<PostResponse> CreatePost(CreatePostRequest request, CancellationToken cancellationToken = default)
    {
        if (request.Title == null || request.Content == null)
        {
            throw new ArgumentException("Title and content cannot be null.");
        }
        var post = new Post
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Content = request.Content,
            UserId = request.AuthorId,
            CreatedAt = DateTime.UtcNow
        };
        await unitOfWork.Posts.AddAsync(post, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        await eventDispatcher.DispatchAsync(
            new PostCreatedDomainEvent(post.Id, post.UserId, post.Title, DateTime.UtcNow),
            cancellationToken);

        return PostDtoMapper.ToDto(post);
    }
}