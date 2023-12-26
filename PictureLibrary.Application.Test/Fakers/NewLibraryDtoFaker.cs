using AutoBogus;
using PictureLibrary.Contracts.Library;

namespace PictureLibrary.Application.Test.Fakers
{
    public class NewLibraryDtoFaker : AutoFaker<NewLibraryDto>
    {
        public NewLibraryDtoFaker()
        {
            RuleFor(x => x.Name, f => f.Lorem.Word());
            RuleFor(x => x.Description, f => f.Lorem.Sentence());
        }
    }
}
