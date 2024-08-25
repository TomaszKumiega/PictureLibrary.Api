using AutoBogus;
using MongoDB.Bson;
using PictureLibrary.Domain.Entities;

namespace PictureLibrary.TestTools.Fakers
{
    public class FileMetadataFaker : AutoFaker<FileMetadata>
    {
        public FileMetadataFaker()
        {
            RuleFor(x => x.Id, x => ObjectId.GenerateNewId());
            RuleFor(x => x.OwnerId, x => ObjectId.GenerateNewId());
        }
    }
}
