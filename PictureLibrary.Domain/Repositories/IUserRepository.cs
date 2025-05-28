using PictureLibrary.Domain.Entities;

namespace PictureLibrary.Domain.Repositories;

public interface IUserRepository : IRepository<User>
{
    User? GetByUsername(string username);
    bool UsernameExists(string username);
    bool EmailExists(string email);
}