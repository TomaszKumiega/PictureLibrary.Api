using MongoDB.Driver;
using PictureLibrary.Domain.Configuration;
using PictureLibrary.Domain.Entities;
using PictureLibrary.Domain.Repositories;

namespace PictureLibrary.Infrastructure.Repositories
{
    public class LibraryRepository : Repository<Library>, ILibraryRepository
    {
        public LibraryRepository(
            IAppSettings appSettings, 
            IMongoClient mongoClient) 
            : base(appSettings, mongoClient)
        {
        }

        protected override string CollectionName => "Libraries";
    }
}
