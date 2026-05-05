using Application.DTO;
using Domain.Entities;

namespace Application.Abstractions;

public interface ICreatePostService
{
    public Task<PostResponse> CreatePost(CreatePostRequest request, CancellationToken cancellationToken = default);
}