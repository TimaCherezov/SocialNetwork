using Application.DTO;
using Domain.Entities;

namespace Application.Mappers;

public static class PostDtoMapper
{
    public static PostResponse ToDto(Post post) => new(post.Id, post.Title, post.Content, post.CreatedAt);

}