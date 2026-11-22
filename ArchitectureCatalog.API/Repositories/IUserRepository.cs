using ArchitectureCatalog.API.Entities;

namespace ArchitectureCatalog.API.Repositories;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(int id);
    Task<User?> GetByUsernameAsync(string username);
    Task<User> CreateAsync(User user);
}
