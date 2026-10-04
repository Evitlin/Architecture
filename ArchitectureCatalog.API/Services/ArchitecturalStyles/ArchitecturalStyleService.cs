using ArchitectureCatalog.API.DTOs.ArchitecturalStyles;
using ArchitectureCatalog.API.Entities;
using ArchitectureCatalog.API.Repositories;

namespace ArchitectureCatalog.API.Services.ArchitecturalStyles;

public class ArchitecturalStyleService : IArchitecturalStyleService
{
    private readonly IArchitecturalStyleRepository _repository;

    public ArchitecturalStyleService(IArchitecturalStyleRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<ArchitecturalStyleDto>> GetAllAsync()
    {
        var styles = await _repository.GetAllAsync();
        return styles.Select(MapToDto);
    }

    public async Task<ArchitecturalStyleDto?> GetByIdAsync(int id)
    {
        var style = await _repository.GetByIdAsync(id);
        return style is null ? null : MapToDto(style);
    }

    public async Task<ArchitecturalStyleDto> CreateAsync(CreateArchitecturalStyleDto dto)
    {
        var style = new ArchitecturalStyle
        {
            Name = dto.Name,
            Description = dto.Description,
            StartYear = dto.StartYear,
            EndYear = dto.EndYear
        };

        var created = await _repository.CreateAsync(style);
        return MapToDto(created);
    }

    public async Task<bool> UpdateAsync(int id, UpdateArchitecturalStyleDto dto)
    {
        var existing = await _repository.GetByIdAsync(id);
        if (existing is null)
        {
            return false;
        }

        existing.Name = dto.Name;
        existing.Description = dto.Description;
        existing.StartYear = dto.StartYear;
        existing.EndYear = dto.EndYear;

        return await _repository.UpdateAsync(existing);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _repository.DeleteAsync(id);
    }

    private static ArchitecturalStyleDto MapToDto(ArchitecturalStyle style)
    {
        return new ArchitecturalStyleDto
        {
            Id = style.Id,
            Name = style.Name,
            Description = style.Description,
            StartYear = style.StartYear,
            EndYear = style.EndYear
        };
    }
}
