using FluentValidation;
using MongoDB.Bson;
using PictureLibrary.Application.DtoValidators;
using PictureLibrary.Contracts;

namespace PictureLibrary.Application.Command
{
    public class UpdateLibraryCommandValidator : AbstractValidator<UpdateLibraryCommand>
    {
        public UpdateLibraryCommandValidator(UpdateLibraryValidator dtoValidator)
        {
            RuleFor(x => x.UserId).NotEmpty().Must(userId => ObjectId.TryParse(userId, out _));
            RuleFor(x => x.LibraryId).NotEmpty().Must(libraryId => ObjectId.TryParse(libraryId, out _));
            RuleFor(x => x.Library).NotEmpty().SetValidator(dtoValidator);
        }
    }
}
