using MongoDB.Bson;

namespace PictureLibrary.Domain.Entities
{
    public class FileMetadata : IEntity
    {
        public ObjectId Id { get; set; }
        public ObjectId OwnerId { get; set; }
        public required string FilePath { get; set; }
        public required string FileName { get; set; }
    }
}
