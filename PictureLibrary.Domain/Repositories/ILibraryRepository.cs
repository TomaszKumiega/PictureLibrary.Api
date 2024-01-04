using MongoDB.Bson;
using PictureLibrary.Domain.Entities;

namespace PictureLibrary.Domain.Repositories
{
    public interface ILibraryRepository : IRepository<Library>
    {
        Task<Library?> Get(ObjectId userId, ObjectId libraryId);
        Task<IEnumerable<Library>> GetAll(ObjectId userId);
        Task Add(Library library);
    }
}
