using AutoBogus;
using PictureLibrary.Contracts;

namespace PictureLibrary.Application.Test.Fakers
{
    public class NewTagDtoFaker : AutoFaker<NewTagDto>
    {
        public NewTagDtoFaker()
        {
            RuleFor(x => x.Name, f => f.Random.String(5));
            RuleFor(x => x.ColorHex, x => "#4287f5");
        }
    }
}
