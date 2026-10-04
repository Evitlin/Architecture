using System.ComponentModel.DataAnnotations;

namespace ArchitectureCatalog.API.DTOs.Comments;

public class UpdateCommentDto
{
    [Required]
    [MinLength(1)]
    public string Content { get; set; } = null!;
}
