using MongoDB.Bson;
using PictureLibrary.Domain.Entities;

namespace PictureLibrary.Domain.Repositories
{
    public interface ITagRepository : IRepository<Tag>
    {
        public Task<IEnumerable<Tag>> GetAll(ObjectId libraryId);
    }
}
