using ArchitectureCatalog.API.DTOs.ArchitecturalStyles;

namespace ArchitectureCatalog.API.Services.ArchitecturalStyles;

public interface IArchitecturalStyleService
{
    Task<IEnumerable<ArchitecturalStyleDto>> GetAllAsync();
    Task<ArchitecturalStyleDto?> GetByIdAsync(int id);
    Task<ArchitecturalStyleDto> CreateAsync(CreateArchitecturalStyleDto dto);
    Task<bool> UpdateAsync(int id, UpdateArchitecturalStyleDto dto);
    Task<bool> DeleteAsync(int id);
}
