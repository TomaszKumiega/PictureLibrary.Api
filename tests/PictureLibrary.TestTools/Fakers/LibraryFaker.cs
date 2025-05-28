using AutoBogus;
using MongoDB.Bson;
using PictureLibrary.Domain.Entities;

namespace PictureLibrary.TestTools.Fakers;

public class LibraryFaker : AutoFaker<Library>
{
    public LibraryFaker()
    {
        RuleFor(x => x.Id, f => ObjectId.GenerateNewId());
        RuleFor(x => x.OwnerId, f => ObjectId.GenerateNewId());
        RuleFor(x => x.Name, f => f.Lorem.Word());
        RuleFor(x => x.Description, f => f.Lorem.Sentence());
    }
}