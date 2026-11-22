using ArchitectureCatalog.API.DTOs.Auth;

namespace ArchitectureCatalog.API.Services.Auth;

public interface IAuthService
{
    Task<LoginResponseDto?> LoginAsync(LoginRequestDto request);
    Task<(LoginResponseDto? Response, bool UsernameTaken)> RegisterAsync(RegisterRequestDto request);
    Task<LoginResponseDto?> RefreshAsync(RefreshTokenRequestDto request);
    Task LogoutAsync(RefreshTokenRequestDto request);
}
