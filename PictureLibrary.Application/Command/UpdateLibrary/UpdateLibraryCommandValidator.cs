using FluentValidation;
using MongoDB.Bson;
using PictureLibrary.Contracts;

namespace PictureLibrary.Application.Command
{
    public class UpdateLibraryCommandValidator : AbstractValidator<UpdateLibraryCommand>
    {
        public UpdateLibraryCommandValidator(AbstractValidator<UpdateLibraryDto> dtoValidator)
        {
            RuleFor(x => x.UserId).NotEmpty().Must(userId => ObjectId.TryParse(userId, out _));
            RuleFor(x => x.LibraryId).NotEmpty().Must(libraryId => ObjectId.TryParse(libraryId, out _));
            RuleFor(x => x.Library).NotEmpty().SetValidator(dtoValidator);
        }
    }
}
