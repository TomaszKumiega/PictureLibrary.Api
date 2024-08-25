using MongoDB.Bson;
using MongoDB.Driver;
using PictureLibrary.Domain.Configuration;
using PictureLibrary.Domain.Entities;
using PictureLibrary.Domain.Repositories;

namespace PictureLibrary.Infrastructure.Repositories
{
    public class ImageFileRepository(IAppSettings appSettings, IMongoClient mongoClient) 
        : Repository<ImageFile>(appSettings, mongoClient), IImageFileRepository
    {
        protected override string CollectionName => "ImageFiles";

        public IEnumerable<ImageFile> GetAllFromLibrary(ObjectId libraryId)
        {
            return Query()
                .Where(x => x.LibraryId == libraryId)
                .ToList();
        }
    }
}
