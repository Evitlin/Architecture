using ArchitectureCatalog.API.Entities;
using Dapper;
using MySqlConnector;

namespace ArchitectureCatalog.API.Repositories;

public class BuildingRepository : IBuildingRepository
{
    private readonly MySqlConnection _connection;

    public BuildingRepository(MySqlConnection connection)
    {
        _connection = connection;
    }

    public async Task<IEnumerable<Building>> GetAllAsync()
    {
        const string sql = """
            SELECT
                id AS Id,
                name AS Name,
                address AS Address,
                city AS City,
                description AS Description,
                architect AS Architect,
                year_built AS YearBuilt,
                architectural_style_id AS ArchitecturalStyleId
            FROM buildings
            """;

        await EnsureOpenAsync();
        return await _connection.QueryAsync<Building>(sql);
    }

    public async Task<Building?> GetByIdAsync(int id)
    {
        const string sql = """
            SELECT
                id AS Id,
                name AS Name,
                address AS Address,
                city AS City,
                description AS Description,
                architect AS Architect,
                year_built AS YearBuilt,
                architectural_style_id AS ArchitecturalStyleId
            FROM buildings
            WHERE id = @Id
            """;

        await EnsureOpenAsync();
        return await _connection.QuerySingleOrDefaultAsync<Building>(sql, new { Id = id });
    }

    public async Task<Building> CreateAsync(Building building)
    {
        const string sql = """
            INSERT INTO buildings
                (name, address, city, description, architect, year_built, architectural_style_id)
            VALUES
                (@Name, @Address, @City, @Description, @Architect, @YearBuilt, @ArchitecturalStyleId);
            SELECT LAST_INSERT_ID();
            """;

        await EnsureOpenAsync();
        var id = await _connection.ExecuteScalarAsync<int>(sql, building);
        building.Id = id;
        return building;
    }

    public async Task<bool> UpdateAsync(Building building)
    {
        const string sql = """
            UPDATE buildings
            SET
                name = @Name,
                address = @Address,
                city = @City,
                description = @Description,
                architect = @Architect,
                year_built = @YearBuilt,
                architectural_style_id = @ArchitecturalStyleId
            WHERE id = @Id
            """;

        await EnsureOpenAsync();
        var rowsAffected = await _connection.ExecuteAsync(sql, building);
        return rowsAffected > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        const string sql = """
            DELETE FROM buildings
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
