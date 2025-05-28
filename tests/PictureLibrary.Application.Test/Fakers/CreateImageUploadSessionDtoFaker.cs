using AutoBogus;
using MongoDB.Bson;
using PictureLibrary.Contracts;

namespace PictureLibrary.Application.Test.Fakers;

public class CreateImageUploadSessionDtoFaker : AutoFaker<CreateImageUploadSessionDto>
{
    public CreateImageUploadSessionDtoFaker()
    {
        RuleFor(x => x.FileName, x => x.Random.String());
        RuleFor(x => x.FileLength, x => 1000);
        RuleFor(x => x.LibraryId, x => ObjectId.GenerateNewId().ToString());
        RuleFor(x => x.TagIds, x => []);
    }

    public CreateImageUploadSessionDtoFaker WithLibraryId(ObjectId id)
    {
        RuleFor(x => x.LibraryId, x => id.ToString());

        return this;
    }

    public CreateImageUploadSessionDtoFaker WithTagIds(IEnumerable<ObjectId> tagIds)
    {
        RuleFor(x => x.TagIds, x => tagIds.Select(t => t.ToString()));

        return this;
    }
}