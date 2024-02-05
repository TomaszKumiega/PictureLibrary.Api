using FluentValidation;
using PictureLibrary.Contracts;

namespace PictureLibrary.Application.Command
{
    public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
    {
        public LoginUserCommandValidator(AbstractValidator<LoginUserDto> dtoValidator)
        {
            RuleFor(x => x.LoginDto).NotNull().SetValidator(dtoValidator);
        }
    }
}
