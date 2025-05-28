using AutoBogus;
using PictureLibrary.Contracts;

namespace PictureLibrary.Application.Test.Fakers;

public class LibraryDtoFaker : AutoFaker<LibraryDto>
{
    public LibraryDtoFaker()
    {
        RuleFor(x => x.Name, s => s.Lorem.Word());
        RuleFor(x => x.Description, s => s.Lorem.Sentence());
    }
}