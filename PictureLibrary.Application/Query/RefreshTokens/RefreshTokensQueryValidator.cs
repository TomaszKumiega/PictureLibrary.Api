using FluentValidation;
using PictureLibrary.Application.DtoValidators;
using PictureLibrary.Contracts;

namespace PictureLibrary.Application.Query
{
    public class RefreshTokensQueryValidator : AbstractValidator<RefreshTokensQuery>
    {
        public RefreshTokensQueryValidator(RefreshAuthorizationDataDtoValidator dtoValidator)
        {
            RuleFor(x => x.AuthorizationDataDto).NotNull().SetValidator(dtoValidator);
        }
    }
}
