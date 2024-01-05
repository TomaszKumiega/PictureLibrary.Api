using MongoDB.Bson;
using MongoDB.Driver;
using PictureLibrary.Domain.Configuration;
using PictureLibrary.Domain.Repositories;
using Tag = PictureLibrary.Domain.Entities.Tag;

namespace PictureLibrary.Infrastructure.Repositories
{
    public class TagRepository : Repository<Tag>, ITagRepository
    {
        public TagRepository(
            IAppSettings appSettings, 
            IMongoClient mongoClient) 
            : base(appSettings, mongoClient)
        {
        }

        protected override string CollectionName => "Tags";

        public async Task<IEnumerable<Tag>> GetAll(ObjectId libraryId)
        {
            return await Task.Run(() =>
            {
                return Query()
                .Where(x => x.LibraryId == libraryId);
            });
        }
    }
}
