using System.ComponentModel.DataAnnotations;

namespace ArchitectureCatalog.API.DTOs.Auth;

public class LoginRequestDto
{
    [Required]
    [MinLength(1)]
    public string Username { get; set; } = null!;

    [Required]
    [MinLength(1)]
    public string Password { get; set; } = null!;
}
