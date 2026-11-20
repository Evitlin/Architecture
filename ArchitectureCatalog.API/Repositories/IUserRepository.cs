using ArchitectureCatalog.API.Entities;

namespace ArchitectureCatalog.API.Repositories;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(int id);
}
