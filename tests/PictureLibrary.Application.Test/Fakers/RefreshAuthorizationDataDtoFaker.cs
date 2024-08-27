using AutoBogus;
using PictureLibrary.Contracts;

namespace PictureLibrary.Application.Test.Fakers
{
    public class RefreshAuthorizationDataDtoFaker : AutoFaker<RefreshAuthorizationDataDto>
    {
        public RefreshAuthorizationDataDtoFaker()
        {
            RuleFor(x => x.AccessToken, x => x.Lorem.Letter(10));
            RuleFor(x => x.RefreshToken, x => x.Lorem.Letter(10));
        }
    }
}
