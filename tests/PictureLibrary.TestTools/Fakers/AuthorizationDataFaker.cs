using AutoBogus;
using MongoDB.Bson;
using PictureLibrary.Domain.Entities;

namespace PictureLibrary.TestTools.Fakers
{
    public class AuthorizationDataFaker : AutoFaker<AuthorizationData>
    {
        public AuthorizationDataFaker()
        {
            RuleFor(x => x.Id, x => ObjectId.GenerateNewId());
            RuleFor(x => x.UserId, x => ObjectId.GenerateNewId());
            RuleFor(x => x.AccessToken, x => x.Lorem.Letter(10));
            RuleFor(x => x.RefreshToken, x => x.Lorem.Letter(10));
            RuleFor(x => x.ExpiryDate, x => x.Date.Past());
        }
    }
}
