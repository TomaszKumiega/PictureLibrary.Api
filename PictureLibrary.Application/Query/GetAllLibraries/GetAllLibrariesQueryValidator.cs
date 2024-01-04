using FluentValidation;
using MongoDB.Bson;

namespace PictureLibrary.Application.Query.GetAllLibraries
{
    public class GetAllLibrariesQueryValidator : AbstractValidator<GetAllLibrariesQuery>
    {
        public GetAllLibrariesQueryValidator()
        {
            RuleFor(x => x.UserId).NotEmpty().Must((userId) => ObjectId.TryParse(userId, out _));
        }
    }
}
