using ArchitectureCatalog.API.Entities;

namespace ArchitectureCatalog.API.Repositories;

public interface IArchitecturalStyleRepository
{
    Task<(IReadOnlyList<ArchitecturalStyle> Items, int TotalItems)> GetPagedAsync(int page, int pageSize);
    Task<ArchitecturalStyle?> GetByIdAsync(int id);
    Task<ArchitecturalStyle> CreateAsync(ArchitecturalStyle style);
    Task<bool> UpdateAsync(ArchitecturalStyle style);
    Task<bool> DeleteAsync(int id);
}
