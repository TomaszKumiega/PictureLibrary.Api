using AutoBogus;
using PictureLibrary.Contracts;

namespace PictureLibrary.Application.Test.Fakers
{
    public class UpdateUserDtoFaker : AutoFaker<UpdateUserDto>
    {
        public UpdateUserDtoFaker()
        {
            RuleFor(x => x.Username, f => f.Name.FirstName());
            RuleFor(x => x.Email, f => f.Internet.Email());
        }
    }
}
