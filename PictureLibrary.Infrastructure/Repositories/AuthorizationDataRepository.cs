using MongoDB.Bson;
using MongoDB.Driver;
using PictureLibrary.Domain.Configuration;
using PictureLibrary.Domain.Entities;
using PictureLibrary.Domain.Repositories;

namespace PictureLibrary.Infrastructure.Repositories
{
    public class AuthorizationDataRepository(IAppSettings appSettings, IMongoClient mongoClient) 
        : Repository<AuthorizationData>(appSettings, mongoClient), IAuthorizationDataRepository
    {
        protected override string CollectionName => "AuthorizationData";

        public AuthorizationData? GetByUserId(ObjectId userId)
        {
            return Query().FirstOrDefault(x => x.UserId == userId);
        }

        public async Task<AuthorizationData> UpsertForUser(AuthorizationData entity)
        {
            var authorizationData = Query().FirstOrDefault(x => x.UserId == entity.UserId);

            if (authorizationData == null)
            {
                await Add(entity);
                
                return entity;
            }
            else
            {
                authorizationData.AccessToken = entity.AccessToken;
                authorizationData.RefreshToken = entity.RefreshToken;
                authorizationData.ExpiryDate = entity.ExpiryDate;

                await Update(authorizationData);

                return authorizationData;
            }
        }
    }
}
