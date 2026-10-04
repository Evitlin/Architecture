using ArchitectureCatalog.API.DTOs.Buildings;
using ArchitectureCatalog.API.DTOs.Common;

namespace ArchitectureCatalog.API.Services.Buildings;

public interface IBuildingService
{
    Task<PagedResult<BuildingDto>> GetAllAsync(int page, int pageSize, string? city, int? architecturalStyleId);
    Task<BuildingDto?> GetByIdAsync(int id);
    Task<BuildingDetailsDto?> GetDetailsAsync(int id);
    Task<BuildingDto?> CreateAsync(CreateBuildingDto dto);
    Task<bool> UpdateAsync(int id, UpdateBuildingDto dto);
    Task<bool> DeleteAsync(int id);
}
