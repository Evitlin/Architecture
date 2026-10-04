using ArchitectureCatalog.API.Entities;

namespace ArchitectureCatalog.API.Repositories;

public interface IBuildingRepository
{
    Task<(IReadOnlyList<Building> Items, int TotalItems)> GetPagedAsync(
        int page,
        int pageSize,
        string? city,
        int? architecturalStyleId);
    Task<Building?> GetByIdAsync(int id);
    Task<Building> CreateAsync(Building building);
    Task<bool> UpdateAsync(Building building);
    Task<bool> DeleteAsync(int id);
    Task<bool> ExistsByArchitecturalStyleIdAsync(int architecturalStyleId);
}
