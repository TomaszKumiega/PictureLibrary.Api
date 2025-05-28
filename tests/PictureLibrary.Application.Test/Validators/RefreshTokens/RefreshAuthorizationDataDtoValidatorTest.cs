using FluentValidation.TestHelper;
using PictureLibrary.Application.DtoValidators;
using PictureLibrary.Contracts;

namespace PictureLibrary.Application.Test.Validators.RefreshTokens;

public class RefreshAuthorizationDataDtoValidatorTest
{
    private readonly RefreshAuthorizationDataDtoValidator _validator;

    public RefreshAuthorizationDataDtoValidatorTest()
    {
        _validator = new RefreshAuthorizationDataDtoValidator();
    }

    [Fact]
    public void Validate_Should_Return_True_When_AccessToken_And_RefreshToken_Are_Not_Empty()
    {
        var dto = new RefreshAuthorizationDataDto
        {
            AccessToken = "accessToken",
            RefreshToken = "refresh"
        };

        var result = _validator.TestValidate(dto);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_Should_Return_False_When_AccessToken_Is_Empty()
    {
        var dto = new RefreshAuthorizationDataDto
        {
            AccessToken = string.Empty,
            RefreshToken = "refresh",
        };

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.AccessToken);
    }

    [Fact]
    public void Validate_Should_Return_False_When_RefreshToken_Is_Empty()
    {
        var dto = new RefreshAuthorizationDataDto
        {
            AccessToken = "accessToken",
            RefreshToken = string.Empty,
        };

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.RefreshToken);
    }
}