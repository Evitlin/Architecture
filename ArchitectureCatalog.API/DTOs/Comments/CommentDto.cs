namespace ArchitectureCatalog.API.DTOs.Comments;

public class CommentDto
{
    public int Id { get; set; }
    public string Content { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public int BuildingId { get; set; }
    public List<Common.LinkDto> Links { get; set; } = [];
}
