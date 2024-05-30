using FluentValidation;
using MongoDB.Bson;
using PictureLibrary.Contracts;

namespace PictureLibrary.Application.DtoValidators
{
    public class CreateImageUploadSessionDtoValidator : AbstractValidator<CreateImageUploadSessionDto>
    {
        public CreateImageUploadSessionDtoValidator()
        {
            RuleFor(x => x.FileName).NotEmpty();
            RuleFor(x => x.FileLength).NotEmpty();
            RuleFor(x => x.LibraryId).NotEmpty().Must(libraryId => ObjectId.TryParse(libraryId, out _));
        }
    }
}
