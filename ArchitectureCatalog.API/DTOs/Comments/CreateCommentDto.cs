namespace ArchitectureCatalog.API.DTOs.Comments;

public class CreateCommentDto
{
    public string Content { get; set; } = null!;
    public int BuildingId { get; set; }
}
