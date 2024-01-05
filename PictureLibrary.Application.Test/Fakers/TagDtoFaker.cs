using AutoBogus;
using MongoDB.Bson;
using PictureLibrary.Contracts;

namespace PictureLibrary.Application.Test.Fakers
{
    public class TagDtoFaker : AutoFaker<TagDto>
    {
        public TagDtoFaker()
        {
            RuleFor(x => x.Id, f => ObjectId.GenerateNewId().ToString());
            RuleFor(x => x.LibraryId, f => ObjectId.GenerateNewId().ToString());
            RuleFor(x => x.Name, f => f.Random.String(5));
            RuleFor(x => x.ColorHex, x => x.Random.String(6));
        }
    }
}
