using AutoBogus;
using MongoDB.Bson;
using PictureLibrary.Contracts;

namespace PictureLibrary.Application.Test;

public class ImageFileDtoFaker : AutoFaker<ImageFileDto>
{
    public ImageFileDtoFaker()
    {
        RuleFor(x => x.Id, f => ObjectId.GenerateNewId().ToString());
        RuleFor(x => x.LibraryId, f => ObjectId.GenerateNewId().ToString());
        RuleFor(x => x.TagIds, f => [ObjectId.GenerateNewId().ToString()]);
        RuleFor(x => x.FileName, f => f.Random.String());
        RuleFor(x => x.Base64Thumbnail, f => f.Random.String());
    }
}