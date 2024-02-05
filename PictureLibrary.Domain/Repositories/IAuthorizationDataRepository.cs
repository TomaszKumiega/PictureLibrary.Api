using MongoDB.Bson;
using PictureLibrary.Domain.Entities;

namespace PictureLibrary.Domain.Repositories
{
    public interface IAuthorizationDataRepository : IRepository<AuthorizationData>
    {
        public AuthorizationData? GetByUserId(ObjectId userId);
    }
}
