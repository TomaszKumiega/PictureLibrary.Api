using AutoBogus;
using PictureLibrary.Contracts;

namespace PictureLibrary.Application.Test.Fakers
{
    public class LoginUserDtoFaker : AutoFaker<LoginUserDto>
    {
        public LoginUserDtoFaker()
        {
            RuleFor(x => x.Username, x => x.Person.UserName);
            RuleFor(x => x.Password, x => x.Random.String(8));
        }
    }
}
