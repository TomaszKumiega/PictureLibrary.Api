using FluentValidation;
using MongoDB.Bson;

namespace PictureLibrary.Application.Command
{
    public class DeleteLibraryCommandValidator : AbstractValidator<DeleteLibraryCommand>
    {
        public DeleteLibraryCommandValidator()
        {
            RuleFor(x => x.UserId).NotEmpty().Must(userId => ObjectId.TryParse(userId, out _));
            RuleFor(x => x.LibraryId).NotEmpty().Must(libraryId => ObjectId.TryParse(libraryId, out _));
        }
    }
}
