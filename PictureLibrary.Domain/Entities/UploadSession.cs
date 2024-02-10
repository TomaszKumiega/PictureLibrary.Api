using MongoDB.Bson;

namespace PictureLibrary.Domain.Entities
{
    public class UploadSession : IEntity
    {
        public ObjectId Id { get; set; }
        public required string FileName { get; set; }
        public required int FileLength { get; set; }
        public required string MissingRanges { get; set; }
    }
}
