using ArchitectureCatalog.API.Entities;

namespace ArchitectureCatalog.API.Repositories;

public interface IBuildingRepository
{
    Task<IEnumerable<Building>> GetAllAsync();
    Task<Building?> GetByIdAsync(int id);
    Task<Building> CreateAsync(Building building);
    Task<bool> UpdateAsync(Building building);
    Task<bool> DeleteAsync(int id);
    Task<bool> ExistsByArchitecturalStyleIdAsync(int architecturalStyleId);
}
