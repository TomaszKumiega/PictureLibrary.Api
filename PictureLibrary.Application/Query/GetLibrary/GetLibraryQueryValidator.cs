using FluentValidation;
using MongoDB.Bson;

namespace PictureLibrary.Application.Query;

public class GetLibraryQueryValidator : AbstractValidator<GetLibraryQuery>
{
    public GetLibraryQueryValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().Must((userId) => ObjectId.TryParse(userId, out _));
        RuleFor(x => x.LibraryId).NotEmpty().Must((libraryId) => ObjectId.TryParse(libraryId, out _));
    }
}