using AutoBogus;
using MongoDB.Bson;
using PictureLibrary.Domain.Entities;

namespace PictureLibrary.TestTools.Fakers;

public class UploadSessionFaker : AutoFaker<UploadSession>
{
    public UploadSessionFaker()
    {
        RuleFor(x => x.Id, x => ObjectId.GenerateNewId());
        RuleFor(x => x.UserId, x => ObjectId.GenerateNewId());
        RuleFor(x => x.FileName, x => x.Random.String());
        RuleFor(x => x.FileLength, x => x.Random.Number());
        RuleFor(x => x.MissingRanges, x => string.Empty);
    }
}