using FluentValidation;
using PictureLibrary.Contracts;

namespace PictureLibrary.Application.Command
{
    public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserCommandValidator(AbstractValidator<UpdateUserDto> dtoValidator)
        {
            RuleFor(x => x.UserDto).NotNull().SetValidator(dtoValidator);
        }
    }
}
