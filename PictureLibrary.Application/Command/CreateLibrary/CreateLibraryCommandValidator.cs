using FluentValidation;
using MongoDB.Bson;
using PictureLibrary.Application.DtoValidators;
using PictureLibrary.Contracts.Library;

namespace PictureLibrary.Application.Command.CreateLibrary
{
    public class CreateLibraryCommandValidator : AbstractValidator<CreateLibraryCommand>
    {
        public CreateLibraryCommandValidator(NewLibraryValidator newLibraryValidator)
        {
            RuleFor(x => x.UserId).NotEmpty().Must(userId => ObjectId.TryParse(userId, out _));
            RuleFor(x => x.NewLibrary).NotNull().SetValidator(newLibraryValidator);
        }
    }
}
