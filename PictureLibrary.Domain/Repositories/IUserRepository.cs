using MongoDB.Bson;
using PictureLibrary.Domain.Entities;

namespace PictureLibrary.Domain.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetById(ObjectId id);
        User? GetByUsername(string username);
    }
}
