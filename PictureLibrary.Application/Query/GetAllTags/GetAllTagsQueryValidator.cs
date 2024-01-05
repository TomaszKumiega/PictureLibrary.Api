using FluentValidation;
using MongoDB.Bson;

namespace PictureLibrary.Application.Query
{
    public class GetAllTagsQueryValidator : AbstractValidator<GetAllTagsQuery>
    {
        public GetAllTagsQueryValidator()
        {
            RuleFor(x => x.UserId).NotEmpty().Must((userId) => ObjectId.TryParse(userId, out _));
            RuleFor(x => x.LibraryId).NotEmpty().Must((libraryId) => ObjectId.TryParse(libraryId, out _));
        }
    }
}
