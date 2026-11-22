using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using ArchitectureCatalog.API.Auth;
using ArchitectureCatalog.API.DTOs.Auth;
using ArchitectureCatalog.API.Entities;
using ArchitectureCatalog.API.Repositories;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace ArchitectureCatalog.API.Services.Auth;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly JwtSettings _jwtSettings;

    public AuthService(
        IUserRepository userRepository,
        IRefreshTokenRepository refreshTokenRepository,
        IOptions<JwtSettings> jwtSettings)
    {
        _userRepository = userRepository;
        _refreshTokenRepository = refreshTokenRepository;
        _jwtSettings = jwtSettings.Value;
    }

    public async Task<LoginResponseDto?> LoginAsync(LoginRequestDto request)
    {
        var user = await _userRepository.GetByUsernameAsync(request.Username);
        if (user is null)
        {
            return null;
        }

        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        {
            return null;
        }

        return await IssueTokensAsync(user);
    }

    public async Task<(LoginResponseDto? Response, bool UsernameTaken)> RegisterAsync(RegisterRequestDto request)
    {
        var existing = await _userRepository.GetByUsernameAsync(request.Username);
        if (existing is not null)
        {
            return (null, true);
        }

        var user = await _userRepository.CreateAsync(new User
        {
            Username = request.Username.Trim(),
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            Role = Roles.User
        });

        var tokens = await IssueTokensAsync(user);
        return (tokens, false);
    }

    public async Task<LoginResponseDto?> RefreshAsync(RefreshTokenRequestDto request)
    {
        var storedToken = await _refreshTokenRepository.GetByTokenAsync(request.RefreshToken);
        if (storedToken is null || storedToken.IsRevoked || storedToken.ExpiresAt <= DateTime.UtcNow)
        {
            return null;
        }

        var user = await _userRepository.GetByIdAsync(storedToken.UserId);
        if (user is null)
        {
            return null;
        }

        // Rotation: old refresh token becomes invalid, new pair is issued.
        await _refreshTokenRepository.RevokeAsync(storedToken.Token);

        return await IssueTokensAsync(user);
    }

    public async Task LogoutAsync(RefreshTokenRequestDto request)
    {
        var storedToken = await _refreshTokenRepository.GetByTokenAsync(request.RefreshToken);
        if (storedToken is null || storedToken.IsRevoked)
        {
            return;
        }

        await _refreshTokenRepository.RevokeAsync(storedToken.Token);
    }

    private async Task<LoginResponseDto> IssueTokensAsync(User user)
    {
        var accessExpiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpiryMinutes);
        var refreshExpiresAt = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpiryDays);

        var accessToken = CreateAccessToken(user, accessExpiresAt);
        var refreshTokenValue = CreateRefreshTokenValue();

        await _refreshTokenRepository.CreateAsync(new RefreshToken
        {
            UserId = user.Id,
            Token = refreshTokenValue,
            ExpiresAt = refreshExpiresAt,
            CreatedAt = DateTime.UtcNow,
            IsRevoked = false
        });

        return new LoginResponseDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshTokenValue,
            AccessTokenExpiresAt = accessExpiresAt,
            RefreshTokenExpiresAt = refreshExpiresAt,
            Username = user.Username,
            Role = user.Role,
            UserId = user.Id
        };
    }

    private string CreateAccessToken(User user, DateTime expiresAt)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.Username),
            new(ClaimTypes.Role, user.Role),
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.UniqueName, user.Username)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: expiresAt,
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private static string CreateRefreshTokenValue()
    {
        var bytes = RandomNumberGenerator.GetBytes(64);
        return Convert.ToBase64String(bytes);
    }
}
