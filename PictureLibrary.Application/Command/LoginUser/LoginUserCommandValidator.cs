using FluentValidation;
using PictureLibrary.Application.DtoValidators;
using PictureLibrary.Contracts;

namespace PictureLibrary.Application.Command;

public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
{
    public LoginUserCommandValidator(LoginUserDtoValidator dtoValidator)
    {
        RuleFor(x => x.LoginDto).NotNull().SetValidator(dtoValidator);
    }
}