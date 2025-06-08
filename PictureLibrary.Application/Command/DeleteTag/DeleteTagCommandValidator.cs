using FluentValidation;
using MongoDB.Bson;

namespace PictureLibrary.Application.Command.DeleteTag;

public class DeleteTagCommandValidator : AbstractValidator<DeleteTagCommand>
{
    public DeleteTagCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().Must(userId => ObjectId.TryParse(userId, out _));
        RuleFor(x => x.TagId).NotEmpty().Must(tagId => ObjectId.TryParse(tagId, out _));
    }
}