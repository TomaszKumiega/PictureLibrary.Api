using FluentValidation;
using MongoDB.Bson;

namespace PictureLibrary.Application.Query;

public class GetUserQueryValidator : AbstractValidator<GetUserQuery>
{
    public GetUserQueryValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().Must((userId) => ObjectId.TryParse(userId, out _));
    }
}