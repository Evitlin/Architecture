namespace ArchitectureCatalog.API.DTOs.ArchitecturalStyles;

public class CreateArchitecturalStyleDto
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public int StartYear { get; set; }
    public int EndYear { get; set; }
}
