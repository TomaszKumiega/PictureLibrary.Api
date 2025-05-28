using FluentValidation;
using PictureLibrary.Contracts;

namespace PictureLibrary.Application.DtoValidators;

public class UpdateLibraryValidator : AbstractValidator<UpdateLibraryDto>
{
    public UpdateLibraryValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
    }
}