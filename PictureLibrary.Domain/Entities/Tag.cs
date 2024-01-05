using MongoDB.Bson;

namespace PictureLibrary.Domain.Entities
{
    public class Tag : IEntity
    {
        public ObjectId Id { get; set; }
        public ObjectId LibraryId { get; set; }
        public required string Name { get; set; }
        public required string ColorHex { get; set; }
    }
}
