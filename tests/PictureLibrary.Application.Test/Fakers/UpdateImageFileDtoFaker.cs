using AutoBogus;
using MongoDB.Bson;
using PictureLibrary.Contracts;

namespace PictureLibrary.Application.Test
{
    public class UpdateImageFileDtoFaker : AutoFaker<UpdateImageFileDto>
    {
        public UpdateImageFileDtoFaker()
        {
            RuleFor(x => x.FileName, f => f.Random.String());
            RuleFor(x => x.LibraryId, f => ObjectId.GenerateNewId().ToString());
            RuleFor(x => x.TagIds, f => [ObjectId.GenerateNewId().ToString()]);
        }
    }
}
