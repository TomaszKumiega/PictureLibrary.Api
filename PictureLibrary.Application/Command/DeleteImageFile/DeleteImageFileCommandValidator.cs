using FluentValidation;
using MongoDB.Bson;

namespace PictureLibrary.Application.Command;

public class DeleteImageFileCommandValidator : AbstractValidator<DeleteImageFileCommand>
{
    public DeleteImageFileCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().Must(userId => ObjectId.TryParse(userId, out _));
        RuleFor(x => x.ImageFileId).NotEmpty().Must(imageFileId => ObjectId.TryParse(imageFileId, out _));
    }
}