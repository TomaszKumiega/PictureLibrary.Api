using FluentValidation;
using MongoDB.Bson;

namespace PictureLibrary.Application.Query;

public class GetImageFileContentQueryValidator : AbstractValidator<GetImageFileContentQuery>
{
    public GetImageFileContentQueryValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().Must((userId) => ObjectId.TryParse(userId, out _));
        RuleFor(x => x.ImageFileId).NotEmpty().Must((imageFileId) => ObjectId.TryParse(imageFileId, out _));
    }
}