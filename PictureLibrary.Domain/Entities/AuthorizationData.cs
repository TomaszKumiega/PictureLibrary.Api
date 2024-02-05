using MongoDB.Bson;

namespace PictureLibrary.Domain.Entities
{
    public class AuthorizationData : IEntity
    {
        public required ObjectId Id { get; set; }
        public required Guid UserId { get; set; }
        public required string AccessToken { get; set; }
        public required string RefreshToken { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
