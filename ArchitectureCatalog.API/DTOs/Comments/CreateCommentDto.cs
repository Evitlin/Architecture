using System.ComponentModel.DataAnnotations;

namespace ArchitectureCatalog.API.DTOs.Comments;

public class CreateCommentDto
{
    [Required]
    [MinLength(1)]
    public string Content { get; set; } = null!;

    [Range(1, int.MaxValue)]
    public int BuildingId { get; set; }
}
