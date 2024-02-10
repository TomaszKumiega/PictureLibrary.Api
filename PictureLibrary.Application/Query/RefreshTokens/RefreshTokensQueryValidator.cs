using FluentValidation;
using PictureLibrary.Contracts;

namespace PictureLibrary.Application.Query
{
    public class RefreshTokensQueryValidator : AbstractValidator<RefreshTokensQuery>
    {
        public RefreshTokensQueryValidator(AbstractValidator<RefreshAuthorizationDataDto> dtoValidator)
        {
            RuleFor(x => x.AuthorizationDataDto).NotNull().SetValidator(dtoValidator);
        }
    }
}
