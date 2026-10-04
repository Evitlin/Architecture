using ArchitectureCatalog.API.DTOs.Comments;
using ArchitectureCatalog.API.Entities;
using ArchitectureCatalog.API.Repositories;

namespace ArchitectureCatalog.API.Services.Comments;

public class CommentService : ICommentService
{
    private readonly ICommentRepository _repository;

    public CommentService(ICommentRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<CommentDto>> GetAllAsync()
    {
        var comments = await _repository.GetAllAsync();
        return comments.Select(MapToDto);
    }

    public async Task<CommentDto?> GetByIdAsync(int id)
    {
        var comment = await _repository.GetByIdAsync(id);
        return comment is null ? null : MapToDto(comment);
    }

    public async Task<CommentDto> CreateAsync(CreateCommentDto dto)
    {
        var comment = new Comment
        {
            Content = dto.Content,
            BuildingId = dto.BuildingId,
            CreatedAt = DateTime.UtcNow
        };

        var created = await _repository.CreateAsync(comment);
        return MapToDto(created);
    }

    public async Task<bool> UpdateAsync(int id, UpdateCommentDto dto)
    {
        var existing = await _repository.GetByIdAsync(id);
        if (existing is null)
        {
            return false;
        }

        existing.Content = dto.Content;

        return await _repository.UpdateAsync(existing);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _repository.DeleteAsync(id);
    }

    private static CommentDto MapToDto(Comment comment)
    {
        return new CommentDto
        {
            Id = comment.Id,
            Content = comment.Content,
            CreatedAt = comment.CreatedAt,
            BuildingId = comment.BuildingId
        };
    }
}
