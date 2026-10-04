using ArchitectureCatalog.API.DTOs.Comments;
using ArchitectureCatalog.API.DTOs.Common;
using ArchitectureCatalog.API.Entities;
using ArchitectureCatalog.API.Repositories;

namespace ArchitectureCatalog.API.Services.Comments;

public class CommentService : ICommentService
{
    private readonly ICommentRepository _commentRepository;
    private readonly IBuildingRepository _buildingRepository;

    public CommentService(
        ICommentRepository commentRepository,
        IBuildingRepository buildingRepository)
    {
        _commentRepository = commentRepository;
        _buildingRepository = buildingRepository;
    }

    public async Task<PagedResult<CommentDto>> GetAllAsync(int page, int pageSize, int? buildingId)
    {
        (page, pageSize) = PagedResult<CommentDto>.Normalize(page, pageSize);

        var (items, totalItems) = await _commentRepository.GetPagedAsync(page, pageSize, buildingId);
        var dtos = items.Select(MapToDto).ToList();

        return PagedResult<CommentDto>.Create(dtos, page, pageSize, totalItems);
    }

    public async Task<CommentDto?> GetByIdAsync(int id)
    {
        var comment = await _commentRepository.GetByIdAsync(id);
        return comment is null ? null : MapToDto(comment);
    }

    public async Task<CommentDto?> CreateAsync(CreateCommentDto dto)
    {
        var building = await _buildingRepository.GetByIdAsync(dto.BuildingId);
        if (building is null)
        {
            return null;
        }

        var comment = new Comment
        {
            Content = dto.Content,
            BuildingId = dto.BuildingId,
            CreatedAt = DateTime.UtcNow
        };

        var created = await _commentRepository.CreateAsync(comment);
        return MapToDto(created);
    }

    public async Task<bool> UpdateAsync(int id, UpdateCommentDto dto)
    {
        var existing = await _commentRepository.GetByIdAsync(id);
        if (existing is null)
        {
            return false;
        }

        existing.Content = dto.Content;

        return await _commentRepository.UpdateAsync(existing);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _commentRepository.DeleteAsync(id);
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
