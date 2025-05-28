using FluentValidation;
using PictureLibrary.Application.DtoValidators;

namespace PictureLibrary.Application.Command;

public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator(NewUserValidator userValidator)
    {
        RuleFor(x => x.NewUser).NotNull().SetValidator(userValidator);
    }
}