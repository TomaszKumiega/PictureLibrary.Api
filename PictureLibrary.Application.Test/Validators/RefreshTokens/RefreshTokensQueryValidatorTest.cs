using FluentValidation.TestHelper;
using PictureLibrary.Application.DtoValidators;
using PictureLibrary.Application.Query;
using PictureLibrary.Application.Test.Fakers;

namespace PictureLibrary.Application.Test.Validators
{
    public class RefreshTokensQueryValidatorTest
    {
        private readonly RefreshTokensQueryValidator _validator;

        public RefreshTokensQueryValidatorTest()
        {
            _validator = new RefreshTokensQueryValidator(new RefreshAuthorizationDataDtoValidator());
        }

        [Fact]
        public void Validate_Should_Return_True_When_AuthorizationDataDto_Is_Valid()
        {
            var dto = new RefreshAuthorizationDataDtoFaker().Generate();

            var result = _validator.TestValidate(new RefreshTokensQuery(dto));

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Validate_Should_Return_False_When_AuthorizationDataDto_Is_Null()
        {
            var result = _validator.TestValidate(new RefreshTokensQuery(null));

            result.ShouldHaveValidationErrorFor(x => x.AuthorizationDataDto);
        }
    }
}
