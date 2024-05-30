using MongoDB.Driver;
using PictureLibrary.Domain.Configuration;
using PictureLibrary.Domain.Entities;
using PictureLibrary.Domain.Repositories;

namespace PictureLibrary.Infrastructure.Repositories
{
    public class ImageFileRepository : Repository<ImageFile>, IImageFileRepository
    {
        protected override string CollectionName => "ImageFiles";

        public ImageFileRepository(IAppSettings appSettings, IMongoClient mongoClient) 
            : base(appSettings, mongoClient)
        {
        }
    }
}
