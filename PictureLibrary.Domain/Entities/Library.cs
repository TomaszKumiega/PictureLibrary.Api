using MongoDB.Bson;

namespace PictureLibrary.Domain.Entities;

public class Library : IEntity
{
    public ObjectId Id { get; set; }
    public ObjectId OwnerId { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
}