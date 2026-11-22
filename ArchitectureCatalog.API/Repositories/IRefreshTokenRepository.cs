using ArchitectureCatalog.API.Entities;

namespace ArchitectureCatalog.API.Repositories;

public interface IRefreshTokenRepository
{
    Task<RefreshToken> CreateAsync(RefreshToken refreshToken);
    Task<RefreshToken?> GetByTokenAsync(string token);
    Task RevokeAsync(string token);
    Task RevokeAllForUserAsync(int userId);
}
