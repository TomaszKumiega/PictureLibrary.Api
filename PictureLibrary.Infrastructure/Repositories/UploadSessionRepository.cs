using MongoDB.Driver;
using PictureLibrary.Domain.Configuration;
using PictureLibrary.Domain.Entities;
using PictureLibrary.Domain.Repositories;

namespace PictureLibrary.Infrastructure.Repositories
{
    public class UploadSessionRepository(IAppSettings appSettings, IMongoClient mongoClient) 
        : Repository<UploadSession>(appSettings, mongoClient), IUploadSessionRepository
    {
        protected override string CollectionName => "UploadSessions";
    }
}
