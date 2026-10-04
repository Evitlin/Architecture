using ArchitectureCatalog.API.DTOs.ArchitecturalStyles;
using ArchitectureCatalog.API.DTOs.Buildings;
using ArchitectureCatalog.API.DTOs.Comments;
using ArchitectureCatalog.API.DTOs.Common;
using ArchitectureCatalog.API.Entities;
using ArchitectureCatalog.API.Repositories;

namespace ArchitectureCatalog.API.Services.Buildings;

public class BuildingService : IBuildingService
{
    private readonly IBuildingRepository _buildingRepository;
    private readonly IArchitecturalStyleRepository _styleRepository;
    private readonly ICommentRepository _commentRepository;

    public BuildingService(
        IBuildingRepository buildingRepository,
        IArchitecturalStyleRepository styleRepository,
        ICommentRepository commentRepository)
    {
        _buildingRepository = buildingRepository;
        _styleRepository = styleRepository;
        _commentRepository = commentRepository;
    }

    public async Task<PagedResult<BuildingDto>> GetAllAsync(
        int page,
        int pageSize,
        string? city,
        int? architecturalStyleId)
    {
        (page, pageSize) = PagedResult<BuildingDto>.Normalize(page, pageSize);

        var (items, totalItems) = await _buildingRepository.GetPagedAsync(
            page,
            pageSize,
            city,
            architecturalStyleId);

        var dtos = items.Select(MapToDto).ToList();
        return PagedResult<BuildingDto>.Create(dtos, page, pageSize, totalItems);
    }

    public async Task<BuildingDto?> GetByIdAsync(int id)
    {
        var building = await _buildingRepository.GetByIdAsync(id);
        return building is null ? null : MapToDto(building);
    }

    public async Task<BuildingDetailsDto?> GetDetailsAsync(int id)
    {
        var building = await _buildingRepository.GetByIdAsync(id);
        if (building is null)
        {
            return null;
        }

        var style = await _styleRepository.GetByIdAsync(building.ArchitecturalStyleId);
        if (style is null)
        {
            return null;
        }

        var comments = await _commentRepository.GetByBuildingIdAsync(id);

        return new BuildingDetailsDto
        {
            Building = MapToDto(building),
            ArchitecturalStyle = new ArchitecturalStyleDto
            {
                Id = style.Id,
                Name = style.Name,
                Description = style.Description,
                StartYear = style.StartYear,
                EndYear = style.EndYear
            },
            Comments = comments.Select(comment => new CommentDto
            {
                Id = comment.Id,
                Content = comment.Content,
                CreatedAt = comment.CreatedAt,
                BuildingId = comment.BuildingId
            }).ToList()
        };
    }

    public async Task<BuildingDto?> CreateAsync(CreateBuildingDto dto)
    {
        var style = await _styleRepository.GetByIdAsync(dto.ArchitecturalStyleId);
        if (style is null)
        {
            return null;
        }

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

        var created = await _buildingRepository.CreateAsync(building);
        return MapToDto(created);
    }

    public async Task<bool> UpdateAsync(int id, UpdateBuildingDto dto)
    {
        var existing = await _buildingRepository.GetByIdAsync(id);
        if (existing is null)
        {
            return false;
        }

        var style = await _styleRepository.GetByIdAsync(dto.ArchitecturalStyleId);
        if (style is null)
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

        return await _buildingRepository.UpdateAsync(existing);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var existing = await _buildingRepository.GetByIdAsync(id);
        if (existing is null)
        {
            return false;
        }

        if (await _commentRepository.ExistsByBuildingIdAsync(id))
        {
            throw new InvalidOperationException("Building has related comments and cannot be deleted.");
        }

        return await _buildingRepository.DeleteAsync(id);
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
