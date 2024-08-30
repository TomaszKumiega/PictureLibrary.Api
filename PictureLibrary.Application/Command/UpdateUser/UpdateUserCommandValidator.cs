using FluentValidation;
using MongoDB.Bson;
using PictureLibrary.Application.DtoValidators;
using PictureLibrary.Contracts;

namespace PictureLibrary.Application.Command
{
    public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserCommandValidator(UpdateUserDtoValidator dtoValidator)
        {
            RuleFor(x => x.UserId).NotEmpty().Must(x => ObjectId.TryParse(x, out _));
            RuleFor(x => x.UserDto).NotNull().SetValidator(dtoValidator);
        }
    }
}
