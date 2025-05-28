using AutoBogus;
using MongoDB.Bson;
using PictureLibrary.Domain.Entities;

namespace PictureLibrary.TestTools.Fakers;

public class UserFaker : AutoFaker<User>
{
    public UserFaker()
    {
        RuleFor(x => x.Id, f => ObjectId.GenerateNewId());
    }
}