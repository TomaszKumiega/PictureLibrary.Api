using MongoDB.Bson;
using PictureLibrary.Domain.Entities;

namespace PictureLibrary.Domain.Repositories
{
    public interface IImageFileRepository : IRepository<ImageFile>
    {
        public IEnumerable<ImageFile> GetAllFromLibrary(ObjectId libraryId);
        public ImageFile? GetImageFile(ObjectId imageFileId);
    }
}
