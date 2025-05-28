using MongoDB.Driver;
using PictureLibrary.Domain.Configuration;
using PictureLibrary.Domain.Entities;
using PictureLibrary.Domain.Repositories;

namespace PictureLibrary.Infrastructure.Repositories;

public class UserRepository(IAppSettings appSettings, IMongoClient mongoClient) 
    : Repository<User>(appSettings, mongoClient), IUserRepository
{
    protected override string CollectionName => "Users";

    public bool EmailExists(string email)
    {
        return Query().Any(u => u.Email == email);
    }

    public User? GetByUsername(string username)
    {
        return Query().FirstOrDefault(u => u.Username == username);
    }

    public bool UsernameExists(string username)
    {
        return Query().Any(u => u.Username == username);
    }
}