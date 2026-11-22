using System.ComponentModel.DataAnnotations;

namespace ArchitectureCatalog.API.DTOs.Auth;

public class RefreshTokenRequestDto
{
    [Required]
    [MinLength(1)]
    public string RefreshToken { get; set; } = null!;
}
