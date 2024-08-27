using AutoBogus;
using PictureLibrary.Contracts;

namespace PictureLibrary.Application.Test.Fakers
{
    public class UpdateLibraryDtoFaker : AutoFaker<UpdateLibraryDto>
    {
        public UpdateLibraryDtoFaker()
        {
            RuleFor(dto => dto.Name, f => f.Random.String2(10));
            RuleFor(dto => dto.Description, f => f.Random.String2(10));
        }
    }
}
