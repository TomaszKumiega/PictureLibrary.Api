using FluentValidation;
using MongoDB.Bson;
using PictureLibrary.Contracts.Library;

namespace PictureLibrary.Application.Command.CreateLibrary
{
    public class CreateLibraryCommandValidator : AbstractValidator<CreateLibraryCommand>
    {
        public CreateLibraryCommandValidator(AbstractValidator<NewLibraryDto> newLibraryValidator)
        {
            RuleFor(x => x.UserId).NotEmpty().Must(userId => ObjectId.TryParse(userId, out _));
            RuleFor(x => x.NewLibrary).NotNull().SetValidator(newLibraryValidator);
        }
    }
}
