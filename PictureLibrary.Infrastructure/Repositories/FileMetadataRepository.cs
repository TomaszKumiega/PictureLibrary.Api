using MongoDB.Driver;
using PictureLibrary.Domain.Configuration;
using PictureLibrary.Domain.Entities;
using PictureLibrary.Domain.Repositories;

namespace PictureLibrary.Infrastructure.Repositories
{
    public class FileMetadataRepository : Repository<FileMetadata>, IFileMetadataRepository
    {
        protected override string CollectionName => "FilesMetadata";

        public FileMetadataRepository(IAppSettings appSettings, IMongoClient mongoClient) : base(appSettings, mongoClient)
        {
        }
    }
}
