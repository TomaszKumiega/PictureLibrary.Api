using MongoDB.Bson;

namespace PictureLibrary.Domain.Entities
{
    public interface IEntity
    {
        public ObjectId Id { get; set; }
    }
}
