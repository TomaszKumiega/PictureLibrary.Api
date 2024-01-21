using MongoDB.Bson;

namespace PictureLibrary.Domain.Entities
{
    public class User : IEntity
    {
        public ObjectId Id { get; set; }
        public required string Username { get; set; }
        public string? Email { get; set; }
        public required byte[] PasswordHash { get; set; }
        public required byte[] PasswordSalt { get; set; }
    }
}
