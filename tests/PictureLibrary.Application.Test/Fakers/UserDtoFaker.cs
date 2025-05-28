using AutoBogus;
using PictureLibrary.Contracts;

namespace PictureLibrary.Application.Test.Fakers;

public class UserDtoFaker : AutoFaker<UserDto>
{
    public UserDtoFaker()
    {
        RuleFor(x => x.Username, x => x.Person.UserName);
        RuleFor(x => x.Email, x => x.Person.Email);
    }
}