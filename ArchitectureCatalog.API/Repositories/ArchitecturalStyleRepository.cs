using ArchitectureCatalog.API.Entities;
using Dapper;
using MySqlConnector;

namespace ArchitectureCatalog.API.Repositories;

public class ArchitecturalStyleRepository : IArchitecturalStyleRepository
{
    private readonly MySqlConnection _connection;

    public ArchitecturalStyleRepository(MySqlConnection connection)
    {
        _connection = connection;
    }

    public async Task<IEnumerable<ArchitecturalStyle>> GetAllAsync()
    {
        const string sql = """
            SELECT
                id AS Id,
                name AS Name,
                description AS Description,
                start_year AS StartYear,
                end_year AS EndYear
            FROM architectural_styles
            """;

        await EnsureOpenAsync();
        return await _connection.QueryAsync<ArchitecturalStyle>(sql);
    }

    public async Task<ArchitecturalStyle?> GetByIdAsync(int id)
    {
        const string sql = """
            SELECT
                id AS Id,
                name AS Name,
                description AS Description,
                start_year AS StartYear,
                end_year AS EndYear
            FROM architectural_styles
            WHERE id = @Id
            """;

        await EnsureOpenAsync();
        return await _connection.QuerySingleOrDefaultAsync<ArchitecturalStyle>(sql, new { Id = id });
    }

    public async Task<ArchitecturalStyle> CreateAsync(ArchitecturalStyle style)
    {
        const string sql = """
            INSERT INTO architectural_styles (name, description, start_year, end_year)
            VALUES (@Name, @Description, @StartYear, @EndYear);
            SELECT LAST_INSERT_ID();
            """;

        await EnsureOpenAsync();
        var id = await _connection.ExecuteScalarAsync<int>(sql, style);
        style.Id = id;
        return style;
    }

    public async Task<bool> UpdateAsync(ArchitecturalStyle style)
    {
        const string sql = """
            UPDATE architectural_styles
            SET
                name = @Name,
                description = @Description,
                start_year = @StartYear,
                end_year = @EndYear
            WHERE id = @Id
            """;

        await EnsureOpenAsync();
        var rowsAffected = await _connection.ExecuteAsync(sql, style);
        return rowsAffected > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        const string sql = """
            DELETE FROM architectural_styles
            WHERE id = @Id
            """;

        await EnsureOpenAsync();
        var rowsAffected = await _connection.ExecuteAsync(sql, new { Id = id });
        return rowsAffected > 0;
    }

    private async Task EnsureOpenAsync()
    {
        if (_connection.State != System.Data.ConnectionState.Open)
        {
            await _connection.OpenAsync();
        }
    }
}
