using FluentValidation;
using MongoDB.Bson;
using PictureLibrary.Application.DtoValidators;

namespace PictureLibrary.Application.Command
{
    public class CreateImageUploadSessionCommandValidator : AbstractValidator<CreateImageUploadSessionCommand>
    {
        public CreateImageUploadSessionCommandValidator(CreateImageUploadSessionDtoValidator dtoValidator)
        {
            RuleFor(x => x.UserId).NotEmpty().Must(userId => ObjectId.TryParse(userId, out _));
            RuleFor(x => x.CreateUploadSessionDto).NotNull().SetValidator(dtoValidator);
        }
    }
}
