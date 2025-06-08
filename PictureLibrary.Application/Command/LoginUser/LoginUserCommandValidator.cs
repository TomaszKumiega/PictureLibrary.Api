using FluentValidation;
using PictureLibrary.Application.DtoValidators;

namespace PictureLibrary.Application.Command.LoginUser;

public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
{
    public LoginUserCommandValidator(LoginUserDtoValidator dtoValidator)
    {
        RuleFor(x => x.LoginDto).NotNull().SetValidator(dtoValidator);
    }
}