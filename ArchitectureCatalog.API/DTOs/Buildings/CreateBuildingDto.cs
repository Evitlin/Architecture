using System.ComponentModel.DataAnnotations;

namespace ArchitectureCatalog.API.DTOs.Buildings;

public class CreateBuildingDto
{
    [Required]
    [MinLength(1)]
    public string Name { get; set; } = null!;

    [Required]
    [MinLength(1)]
    public string Address { get; set; } = null!;

    [Required]
    [MinLength(1)]
    public string City { get; set; } = null!;

    [Required]
    [MinLength(1)]
    public string Description { get; set; } = null!;

    public string? Architect { get; set; }

    public int YearBuilt { get; set; }

    [Range(1, int.MaxValue)]
    public int ArchitecturalStyleId { get; set; }
}
