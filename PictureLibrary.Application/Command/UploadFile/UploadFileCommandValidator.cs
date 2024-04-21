using FluentValidation;
using MongoDB.Bson;

namespace PictureLibrary.Application.Command
{
    public class UploadFileCommandValidator : AbstractValidator<UploadFileCommand>
    {
        public UploadFileCommandValidator()
        {
            RuleFor(x => x.UserId).NotEmpty().Must(x => ObjectId.TryParse(x, out _));
            RuleFor(x => x.ContentRange).NotNull();
        }
    }
}
