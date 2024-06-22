using AutoBogus;
using MongoDB.Bson;
using PictureLibrary.Domain.Entities;

namespace PictureLibrary.TestTools.Fakers
{
    public class ImageFileFaker : AutoFaker<ImageFile>
    {
        public ImageFileFaker()
        {
            RuleFor(x => x.Id, f => ObjectId.GenerateNewId());
            RuleFor(x => x.LibraryId, f => ObjectId.GenerateNewId());
            RuleFor(x => x.FileId, f => ObjectId.GenerateNewId());
            RuleFor(x => x.TagIds, f => [ObjectId.GenerateNewId()]);
            RuleFor(x => x.UploadSessionId, f => ObjectId.GenerateNewId());
        }
    }
}
