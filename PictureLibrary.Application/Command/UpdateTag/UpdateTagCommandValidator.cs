using FluentValidation;
using MongoDB.Bson;
using PictureLibrary.Application.DtoValidators;

namespace PictureLibrary.Application.Command.UpdateTag;

public class UpdateTagCommandValidator : AbstractValidator<UpdateTagCommand>
{
    public UpdateTagCommandValidator(UpdateTagValidator dtoValidator)
    {
        RuleFor(x => x.UserId).NotEmpty().Must(userId => ObjectId.TryParse(userId, out _));
        RuleFor(x => x.TagId).NotEmpty().Must(tagId => ObjectId.TryParse(tagId, out _));
        RuleFor(x => x.UpdateTagDto).NotNull().SetValidator(dtoValidator);
    }
}