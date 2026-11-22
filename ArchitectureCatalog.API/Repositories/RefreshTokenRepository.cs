using ArchitectureCatalog.API.Entities;
using Dapper;
using MySqlConnector;

namespace ArchitectureCatalog.API.Repositories;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly MySqlConnection _connection;

    public RefreshTokenRepository(MySqlConnection connection)
    {
        _connection = connection;
    }

    public async Task<RefreshToken> CreateAsync(RefreshToken refreshToken)
    {
        const string sql = """
            INSERT INTO refresh_tokens (user_id, token, expires_at, created_at, is_revoked)
            VALUES (@UserId, @Token, @ExpiresAt, @CreatedAt, @IsRevoked);
            SELECT LAST_INSERT_ID();
            """;

        await EnsureOpenAsync();
        var id = await _connection.ExecuteScalarAsync<int>(sql, refreshToken);
        refreshToken.Id = id;
        return refreshToken;
    }

    public async Task<RefreshToken?> GetByTokenAsync(string token)
    {
        const string sql = """
            SELECT
                id AS Id,
                user_id AS UserId,
                token AS Token,
                expires_at AS ExpiresAt,
                created_at AS CreatedAt,
                is_revoked AS IsRevoked
            FROM refresh_tokens
            WHERE token = @Token
            """;

        await EnsureOpenAsync();
        return await _connection.QuerySingleOrDefaultAsync<RefreshToken>(sql, new { Token = token });
    }

    public async Task RevokeAsync(string token)
    {
        const string sql = """
            UPDATE refresh_tokens
            SET is_revoked = TRUE
            WHERE token = @Token
            """;

        await EnsureOpenAsync();
        await _connection.ExecuteAsync(sql, new { Token = token });
    }

    public async Task RevokeAllForUserAsync(int userId)
    {
        const string sql = """
            UPDATE refresh_tokens
            SET is_revoked = TRUE
            WHERE user_id = @UserId AND is_revoked = FALSE
            """;

        await EnsureOpenAsync();
        await _connection.ExecuteAsync(sql, new { UserId = userId });
    }

    private async Task EnsureOpenAsync()
    {
        if (_connection.State != System.Data.ConnectionState.Open)
        {
            await _connection.OpenAsync();
        }
    }
}
