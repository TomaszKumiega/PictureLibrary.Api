using MongoDB.Bson;
using MongoDB.Driver;
using PictureLibrary.Domain.Configuration;
using PictureLibrary.Domain.Entities;
using PictureLibrary.Domain.Repositories;

namespace PictureLibrary.Infrastructure.Repositories
{
    public class UserRepository(IAppSettings appSettings, IMongoClient mongoClient) 
        : Repository<User>(appSettings, mongoClient), IUserRepository
    {
        protected override string CollectionName => "Users";

        public User? GetByUsername(string username)
        {
            return Query().FirstOrDefault(u => u.Username == username);
        }
    }
}
