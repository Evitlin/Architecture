using ArchitectureCatalog.API.Entities;
using Dapper;
using MySqlConnector;

namespace ArchitectureCatalog.API.Repositories;

public class UserRepository : IUserRepository
{
    private readonly MySqlConnection _connection;

    public UserRepository(MySqlConnection connection)
    {
        _connection = connection;
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        const string sql = """
            SELECT
                id AS Id,
                username AS Username,
                password_hash AS PasswordHash,
                role AS Role
            FROM users
            WHERE id = @Id
            """;

        await EnsureOpenAsync();
        return await _connection.QuerySingleOrDefaultAsync<User>(sql, new { Id = id });
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        const string sql = """
            SELECT
                id AS Id,
                username AS Username,
                password_hash AS PasswordHash,
                role AS Role
            FROM users
            WHERE username = @Username
            """;

        await EnsureOpenAsync();
        return await _connection.QuerySingleOrDefaultAsync<User>(sql, new { Username = username });
    }

    public async Task<User> CreateAsync(User user)
    {
        const string sql = """
            INSERT INTO users (username, password_hash, role)
            VALUES (@Username, @PasswordHash, @Role);
            SELECT LAST_INSERT_ID();
            """;

        await EnsureOpenAsync();
        user.Id = await _connection.ExecuteScalarAsync<int>(sql, user);
        return user;
    }

    private async Task EnsureOpenAsync()
    {
        if (_connection.State != System.Data.ConnectionState.Open)
        {
            await _connection.OpenAsync();
        }
    }
}
