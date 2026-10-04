namespace ArchitectureCatalog.API.DTOs.Common;

public class LinkDto
{
    public string Rel { get; set; } = null!;
    public string Href { get; set; } = null!;
    public string Method { get; set; } = null!;

    public LinkDto()
    {
    }

    public LinkDto(string rel, string href, string method)
    {
        Rel = rel;
        Href = href;
        Method = method;
    }
}
