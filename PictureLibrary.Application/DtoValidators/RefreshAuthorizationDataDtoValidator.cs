using FluentValidation;
using PictureLibrary.Contracts;

namespace PictureLibrary.Application.DtoValidators
{
    public class RefreshAuthorizationDataDtoValidator : AbstractValidator<RefreshAuthorizationDataDto>
    {
        public RefreshAuthorizationDataDtoValidator()
        {
            RuleFor(x => x.AccessToken).NotEmpty();
            RuleFor(x => x.RefreshToken).NotEmpty();
        }
    }
}
