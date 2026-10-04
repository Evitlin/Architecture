using ArchitectureCatalog.API.DTOs.Buildings;
using ArchitectureCatalog.API.Entities;
using ArchitectureCatalog.API.Repositories;

namespace ArchitectureCatalog.API.Services.Buildings;

public class BuildingService : IBuildingService
{
    private readonly IBuildingRepository _repository;

    public BuildingService(IBuildingRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<BuildingDto>> GetAllAsync()
    {
        var buildings = await _repository.GetAllAsync();
        return buildings.Select(MapToDto);
    }

    public async Task<BuildingDto?> GetByIdAsync(int id)
    {
        var building = await _repository.GetByIdAsync(id);
        return building is null ? null : MapToDto(building);
    }

    public async Task<BuildingDto> CreateAsync(CreateBuildingDto dto)
    {
        var building = new Building
        {
            Name = dto.Name,
            Address = dto.Address,
            City = dto.City,
            Description = dto.Description,
            Architect = dto.Architect,
            YearBuilt = dto.YearBuilt,
            ArchitecturalStyleId = dto.ArchitecturalStyleId
        };

        var created = await _repository.CreateAsync(building);
        return MapToDto(created);
    }

    public async Task<bool> UpdateAsync(int id, UpdateBuildingDto dto)
    {
        var existing = await _repository.GetByIdAsync(id);
        if (existing is null)
        {
            return false;
        }

        existing.Name = dto.Name;
        existing.Address = dto.Address;
        existing.City = dto.City;
        existing.Description = dto.Description;
        existing.Architect = dto.Architect;
        existing.YearBuilt = dto.YearBuilt;
        existing.ArchitecturalStyleId = dto.ArchitecturalStyleId;

        return await _repository.UpdateAsync(existing);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _repository.DeleteAsync(id);
    }

    private static BuildingDto MapToDto(Building building)
    {
        return new BuildingDto
        {
            Id = building.Id,
            Name = building.Name,
            Address = building.Address,
            City = building.City,
            Description = building.Description,
            Architect = building.Architect,
            YearBuilt = building.YearBuilt,
            ArchitecturalStyleId = building.ArchitecturalStyleId
        };
    }
}
