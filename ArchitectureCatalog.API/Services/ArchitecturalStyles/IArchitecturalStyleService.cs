using ArchitectureCatalog.API.DTOs.ArchitecturalStyles;
using ArchitectureCatalog.API.DTOs.Common;

namespace ArchitectureCatalog.API.Services.ArchitecturalStyles;

public interface IArchitecturalStyleService
{
    Task<PagedResult<ArchitecturalStyleDto>> GetAllAsync(int page, int pageSize);
    Task<ArchitecturalStyleDto?> GetByIdAsync(int id);
    Task<ArchitecturalStyleDto> CreateAsync(CreateArchitecturalStyleDto dto);
    Task<bool> UpdateAsync(int id, UpdateArchitecturalStyleDto dto);
    Task<bool> DeleteAsync(int id);
}
