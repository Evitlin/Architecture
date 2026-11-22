using System.ComponentModel.DataAnnotations;

namespace ArchitectureCatalog.API.DTOs.Auth;

public class RegisterRequestDto
{
    [Required]
    [MinLength(3)]
    [MaxLength(255)]
    public string Username { get; set; } = null!;

    [Required]
    [MinLength(6)]
    [MaxLength(100)]
    public string Password { get; set; } = null!;
}
