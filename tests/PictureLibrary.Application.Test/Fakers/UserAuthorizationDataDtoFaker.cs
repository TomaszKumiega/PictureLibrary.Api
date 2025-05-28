using AutoBogus;
using MongoDB.Bson;
using PictureLibrary.Contracts;

namespace PictureLibrary.Application.Test.Fakers;

public class UserAuthorizationDataDtoFaker : AutoFaker<UserAuthorizationDataDto>
{
    public UserAuthorizationDataDtoFaker()
    {
        RuleFor(x => x.UserId, x => ObjectId.GenerateNewId().ToString());
        RuleFor(x => x.AccessToken, x => x.Lorem.Letter(10));
        RuleFor(x => x.RefreshToken, x => x.Lorem.Letter(10));
        RuleFor(x => x.ExpiryDate, x => x.Date.Past());
    }
}