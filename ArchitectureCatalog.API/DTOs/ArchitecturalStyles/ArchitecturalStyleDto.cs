namespace ArchitectureCatalog.API.DTOs.ArchitecturalStyles;

public class ArchitecturalStyleDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public int StartYear { get; set; }
    public int EndYear { get; set; }
    public List<Common.LinkDto> Links { get; set; } = [];
}
