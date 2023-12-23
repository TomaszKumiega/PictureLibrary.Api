using MongoDB.Bson;

namespace PictureLibrary.Domain.Entities
{
    public class Library
    {
        public ObjectId Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
    }
}
