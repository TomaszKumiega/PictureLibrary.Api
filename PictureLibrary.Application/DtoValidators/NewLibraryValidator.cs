using FluentValidation;
using PictureLibrary.Contracts.Library;

namespace PictureLibrary.Application.DtoValidators;

public class NewLibraryValidator : AbstractValidator<NewLibraryDto>
{
    public NewLibraryValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name cannot be empty");
    }
}