using AutoBogus;
using MongoDB.Bson;
using PictureLibrary.Domain.Entities;

namespace PictureLibrary.TestTools.Fakers
{
    public class ImageFileFaker : AutoFaker<ImageFile>
    {
        public ImageFileFaker()
        {
            RuleFor(x => x.Id, x => ObjectId.GenerateNewId());
            RuleFor(x => x.LibraryId, x => ObjectId.GenerateNewId());
            RuleFor(x => x.FileId, x => ObjectId.GenerateNewId());
            RuleFor(x => x.TagIds, x => [ObjectId.GenerateNewId()]);
        }
    }
}
