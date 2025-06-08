using FluentValidation;
using PictureLibrary.Application.DtoValidators;

namespace PictureLibrary.Application.Query.RefreshTokens;

public class RefreshTokensQueryValidator : AbstractValidator<RefreshTokensQuery>
{
    public RefreshTokensQueryValidator(RefreshAuthorizationDataDtoValidator dtoValidator)
    {
        RuleFor(x => x.AuthorizationDataDto).NotNull().SetValidator(dtoValidator);
    }
}