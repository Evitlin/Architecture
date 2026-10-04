using ArchitectureCatalog.API.DTOs.Comments;
using ArchitectureCatalog.API.DTOs.Common;

namespace ArchitectureCatalog.API.Services.Comments;

public interface ICommentService
{
    Task<PagedResult<CommentDto>> GetAllAsync(int page, int pageSize, int? buildingId);
    Task<IReadOnlyList<CommentDto>?> GetByStyleAndBuildingAsync(int styleId, int buildingId);
    Task<CommentDto?> GetByIdAsync(int id);
    Task<CommentDto?> CreateAsync(CreateCommentDto dto);
    Task<bool> UpdateAsync(int id, UpdateCommentDto dto);
    Task<bool> DeleteAsync(int id);
}
