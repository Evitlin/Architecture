using ArchitectureCatalog.API.Entities;
using Dapper;
using MySqlConnector;

namespace ArchitectureCatalog.API.Repositories;

public class CommentRepository : ICommentRepository
{
    private readonly MySqlConnection _connection;

    public CommentRepository(MySqlConnection connection)
    {
        _connection = connection;
    }

    public async Task<IEnumerable<Comment>> GetAllAsync()
    {
        const string sql = """
            SELECT
                id AS Id,
                content AS Content,
                created_at AS CreatedAt,
                building_id AS BuildingId
            FROM comments
            """;

        await EnsureOpenAsync();
        return await _connection.QueryAsync<Comment>(sql);
    }

    public async Task<Comment?> GetByIdAsync(int id)
    {
        const string sql = """
            SELECT
                id AS Id,
                content AS Content,
                created_at AS CreatedAt,
                building_id AS BuildingId
            FROM comments
            WHERE id = @Id
            """;

        await EnsureOpenAsync();
        return await _connection.QuerySingleOrDefaultAsync<Comment>(sql, new { Id = id });
    }

    public async Task<Comment> CreateAsync(Comment comment)
    {
        const string sql = """
            INSERT INTO comments (content, created_at, building_id)
            VALUES (@Content, @CreatedAt, @BuildingId);
            SELECT LAST_INSERT_ID();
            """;

        await EnsureOpenAsync();
        var id = await _connection.ExecuteScalarAsync<int>(sql, comment);
        comment.Id = id;
        return comment;
    }

    public async Task<bool> UpdateAsync(Comment comment)
    {
        const string sql = """
            UPDATE comments
            SET
                content = @Content,
                created_at = @CreatedAt,
                building_id = @BuildingId
            WHERE id = @Id
            """;

        await EnsureOpenAsync();
        var rowsAffected = await _connection.ExecuteAsync(sql, comment);
        return rowsAffected > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        const string sql = """
            DELETE FROM comments
            WHERE id = @Id
            """;

        await EnsureOpenAsync();
        var rowsAffected = await _connection.ExecuteAsync(sql, new { Id = id });
        return rowsAffected > 0;
    }

    public async Task<bool> ExistsByBuildingIdAsync(int buildingId)
    {
        const string sql = """
            SELECT EXISTS(
                SELECT 1
                FROM comments
                WHERE building_id = @BuildingId
            )
            """;

        await EnsureOpenAsync();
        return await _connection.ExecuteScalarAsync<bool>(sql, new { BuildingId = buildingId });
    }

    private async Task EnsureOpenAsync()
    {
        if (_connection.State != System.Data.ConnectionState.Open)
        {
            await _connection.OpenAsync();
        }
    }
}
