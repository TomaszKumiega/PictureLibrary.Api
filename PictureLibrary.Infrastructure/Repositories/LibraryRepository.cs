using MongoDB.Bson;
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

        public async Task<Library?> Get(ObjectId userId, ObjectId libraryId)
        {
            return await Task.Run(() =>
             Query().
             FirstOrDefault(l => l.Id == libraryId && l.OwnerId == userId));
        }

        public async Task Add(Library library)
        {
            await _collection.InsertOneAsync(library);
        }   
    }
}
