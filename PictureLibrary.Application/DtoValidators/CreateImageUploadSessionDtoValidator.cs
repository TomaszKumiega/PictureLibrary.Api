using FluentValidation;
using PictureLibrary.Contracts;

namespace PictureLibrary.Application.DtoValidators
{
    public class CreateImageUploadSessionDtoValidator : AbstractValidator<CreateImageUploadSessionDto>
    {
        public CreateImageUploadSessionDtoValidator()
        {
            RuleFor(x => x.FileName).NotEmpty();
            RuleFor(x => x.FileLength).NotEmpty();
        }
    }
}
