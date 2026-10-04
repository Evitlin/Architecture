namespace ArchitectureCatalog.API.DTOs.Buildings;

public class BuildingDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Address { get; set; } = null!;
    public string City { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string? Architect { get; set; }
    public int YearBuilt { get; set; }
    public int ArchitecturalStyleId { get; set; }
}
