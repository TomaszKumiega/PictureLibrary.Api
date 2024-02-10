using AutoBogus;
using PictureLibrary.Contracts;

namespace PictureLibrary.Application.Test.Fakers
{
    public class CreateImageUploadSessionDtoFaker : AutoFaker<CreateImageUploadSessionDto>
    {
        public CreateImageUploadSessionDtoFaker()
        {
            RuleFor(x => x.FileName, x => x.Random.String());
            RuleFor(x => x.FileLength, x => x.Random.Number());
        }
    }
}
