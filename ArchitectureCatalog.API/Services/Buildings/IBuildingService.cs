using ArchitectureCatalog.API.DTOs.Buildings;

namespace ArchitectureCatalog.API.Services.Buildings;

public interface IBuildingService
{
    Task<IEnumerable<BuildingDto>> GetAllAsync();
    Task<BuildingDto?> GetByIdAsync(int id);
    Task<BuildingDto> CreateAsync(CreateBuildingDto dto);
    Task<bool> UpdateAsync(int id, UpdateBuildingDto dto);
    Task<bool> DeleteAsync(int id);
}
