using FluentValidation;
using MongoDB.Bson;
using PictureLibrary.Contracts;

namespace PictureLibrary.Application;

public class UpdateImageFileDtoValidator : AbstractValidator<UpdateImageFileDto>
{
    public UpdateImageFileDtoValidator()
    {
        RuleFor(x => x.FileName).Must(f => f == null || f.Trim() != string.Empty);
        RuleFor(x => x.LibraryId).NotEmpty().Must(libraryId => ObjectId.TryParse(libraryId, out _));
        RuleFor(x => x.TagIds).Must(tagIds => tagIds == null || tagIds.All(tagId => ObjectId.TryParse(tagId, out _)));
    }
}