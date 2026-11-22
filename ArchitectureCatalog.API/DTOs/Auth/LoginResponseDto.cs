namespace ArchitectureCatalog.API.DTOs.Auth;

public class LoginResponseDto
{
    public string AccessToken { get; set; } = null!;
    public string RefreshToken { get; set; } = null!;
    public DateTime AccessTokenExpiresAt { get; set; }
    public DateTime RefreshTokenExpiresAt { get; set; }
    public string Username { get; set; } = null!;
    public string Role { get; set; } = null!;
    public int UserId { get; set; }
}
