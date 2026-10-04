using ArchitectureCatalog.API.Entities;

namespace ArchitectureCatalog.API.Repositories;

public interface ICommentRepository
{
    Task<(IReadOnlyList<Comment> Items, int TotalItems)> GetPagedAsync(
        int page,
        int pageSize,
        int? buildingId);
    Task<IReadOnlyList<Comment>> GetByBuildingIdAsync(int buildingId);
    Task<Comment?> GetByIdAsync(int id);
    Task<Comment> CreateAsync(Comment comment);
    Task<bool> UpdateAsync(Comment comment);
    Task<bool> DeleteAsync(int id);
    Task<bool> ExistsByBuildingIdAsync(int buildingId);
}
