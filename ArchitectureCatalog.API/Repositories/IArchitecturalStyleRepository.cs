using ArchitectureCatalog.API.Entities;

namespace ArchitectureCatalog.API.Repositories;

public interface IArchitecturalStyleRepository
{
    Task<IEnumerable<ArchitecturalStyle>> GetAllAsync();
    Task<ArchitecturalStyle?> GetByIdAsync(int id);
    Task<ArchitecturalStyle> CreateAsync(ArchitecturalStyle style);
    Task<bool> UpdateAsync(ArchitecturalStyle style);
    Task<bool> DeleteAsync(int id);
}
