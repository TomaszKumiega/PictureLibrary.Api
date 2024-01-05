using FluentValidation;
using MongoDB.Bson;
using PictureLibrary.Contracts;

namespace PictureLibrary.Application.Command.UpdateTag
{
    public class UpdateTagCommandValidator : AbstractValidator<UpdateTagCommand>
    {
        public UpdateTagCommandValidator(AbstractValidator<UpdateTagDto> dtoValidator)
        {
            RuleFor(x => x.UserId).NotEmpty().Must(userId => ObjectId.TryParse(userId, out _));
            RuleFor(x => x.TagId).NotEmpty().Must(tagId => ObjectId.TryParse(tagId, out _));
            RuleFor(x => x.UpdateTagDto).NotNull().SetValidator(dtoValidator);
        }
    }
}
