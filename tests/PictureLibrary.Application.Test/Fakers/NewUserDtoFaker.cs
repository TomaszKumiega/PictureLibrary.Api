using AutoBogus;
using PictureLibrary.Contracts;

namespace PictureLibrary.Application.Test.Fakers;

public class NewUserDtoFaker : AutoFaker<NewUserDto>
{
    public NewUserDtoFaker()
    {
        RuleFor(x => x.Username, x => x.Person.UserName);
        RuleFor(x => x.Email, x => x.Person.Email);
        RuleFor(x => x.Password, x => x.Random.String(10));
    }
}