using MongoDB.Bson;
using MongoDB.Driver;
using PictureLibrary.Domain.Configuration;
using PictureLibrary.Domain.Entities;
using PictureLibrary.Domain.Repositories;

namespace PictureLibrary.Infrastructure.Repositories
{
    public class AuthorizationDataRepository : Repository<AuthorizationData>, IAuthorizationDataRepository
    {
        public AuthorizationDataRepository(
            IAppSettings appSettings, 
            IMongoClient mongoClient) 
            : base(appSettings, mongoClient)
        {
        }

        protected override string CollectionName => "AuthorizationData";

        public AuthorizationData? GetByUserId(ObjectId userId)
        {
            return Query().FirstOrDefault(x => x.UserId == userId);
        }
    }
}
