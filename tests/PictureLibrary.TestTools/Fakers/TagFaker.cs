using AutoBogus;
using MongoDB.Bson;
using PictureLibrary.Domain.Entities;

namespace PictureLibrary.TestTools.Fakers;

public class TagFaker : AutoFaker<Tag>
{
    public TagFaker()
    {
        RuleFor(x => x.Id, f => ObjectId.GenerateNewId());
        RuleFor(x => x.LibraryId, f => ObjectId.GenerateNewId());
        RuleFor(x => x.Name, f => f.Random.String(5));
        RuleFor(x => x.ColorHex, x => x.Random.String(6));
    }
}