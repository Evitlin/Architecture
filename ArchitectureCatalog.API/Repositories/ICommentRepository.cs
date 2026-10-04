using ArchitectureCatalog.API.Entities;

namespace ArchitectureCatalog.API.Repositories;

public interface ICommentRepository
{
    Task<IEnumerable<Comment>> GetAllAsync();
    Task<Comment?> GetByIdAsync(int id);
    Task<Comment> CreateAsync(Comment comment);
    Task<bool> UpdateAsync(Comment comment);
    Task<bool> DeleteAsync(int id);
    Task<bool> ExistsByBuildingIdAsync(int buildingId);
}
