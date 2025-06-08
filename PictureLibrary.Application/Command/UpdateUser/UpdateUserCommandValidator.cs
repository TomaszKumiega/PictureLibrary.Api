using FluentValidation;
using MongoDB.Bson;
using PictureLibrary.Application.DtoValidators;

namespace PictureLibrary.Application.Command.UpdateUser;

public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator(UpdateUserDtoValidator dtoValidator)
    {
        RuleFor(x => x.UserId).NotEmpty().Must(x => ObjectId.TryParse(x, out _));
        RuleFor(x => x.UserDto).NotNull().SetValidator(dtoValidator);
    }
}