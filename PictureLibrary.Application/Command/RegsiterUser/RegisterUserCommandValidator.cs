using FluentValidation;
using PictureLibrary.Contracts;

namespace PictureLibrary.Application.Command
{
    public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
    {
        public RegisterUserCommandValidator(AbstractValidator<NewUserDto> userValidator)
        {
            RuleFor(x => x.NewUser).NotNull().SetValidator(userValidator);
        }
    }
}
