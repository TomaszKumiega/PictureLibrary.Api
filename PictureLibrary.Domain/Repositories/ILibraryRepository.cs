using MongoDB.Bson;
using PictureLibrary.Domain.Entities;

namespace PictureLibrary.Domain.Repositories
{
    public interface ILibraryRepository : IRepository<Library>
    {
        Task<Library?> Get(ObjectId userId, ObjectId libraryId);
        Task Add(Library library);
    }
}
