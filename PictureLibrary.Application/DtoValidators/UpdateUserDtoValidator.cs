using FluentValidation;
using PictureLibrary.Contracts;

namespace PictureLibrary.Application.DtoValidators;

public class UpdateUserDtoValidator : AbstractValidator<UpdateUserDto>
{
    public UpdateUserDtoValidator()
    {
        RuleFor(x => x.Username).NotEmpty();
        When(x => x.Email != null, () =>
        {
            RuleFor(x => x.Email).EmailAddress();
        });
    }
}