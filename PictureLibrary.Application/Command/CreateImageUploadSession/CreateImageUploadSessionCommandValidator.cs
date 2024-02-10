using FluentValidation;
using MongoDB.Bson;
using PictureLibrary.Contracts;

namespace PictureLibrary.Application.Command
{
    public class CreateImageUploadSessionCommandValidator : AbstractValidator<CreateImageUploadSessionCommand>
    {
        public CreateImageUploadSessionCommandValidator(AbstractValidator<CreateImageUploadSessionDto> dtoValidator)
        {
            RuleFor(x => x.UserId).NotEmpty().Must(userId => ObjectId.TryParse(userId, out _));
            RuleFor(x => x.CreateUploadSessionDto).NotNull().SetValidator(dtoValidator);
        }
    }
}
