using ArchitectureCatalog.API.DTOs.ArchitecturalStyles;
using ArchitectureCatalog.API.DTOs.Comments;
using ArchitectureCatalog.API.DTOs.Common;

namespace ArchitectureCatalog.API.DTOs.Buildings;

public class BuildingDetailsDto
{
    public BuildingDto Building { get; set; } = null!;
    public ArchitecturalStyleDto ArchitecturalStyle { get; set; } = null!;
    public IReadOnlyList<CommentDto> Comments { get; set; } = Array.Empty<CommentDto>();
    public List<LinkDto> Links { get; set; } = [];
}
