using FluentValidation.TestHelper;
using PictureLibrary.Application.DtoValidators;
using PictureLibrary.Contracts;

namespace PictureLibrary.Application.Test.Validators.LoginUser;

public class LoginUserDtoValidatorTest
{
    private readonly LoginUserDtoValidator _validator;

    public LoginUserDtoValidatorTest()
    {
        _validator = new LoginUserDtoValidator();
    }

    [Fact]
    public void Validate_Should_Have_Errors_When_Username_Is_Null()
    {
        var dto = new LoginUserDto()
        {
            Username = null!,
            Password = "password"
        };

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.Username);
    }

    [Fact]
    public void Validate_Should_Have_Errors_When_Username_Is_Empty()
    {
        var dto = new LoginUserDto
        {
            Username = string.Empty,
            Password = "password"
        };

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.Username);
    }

    [Fact]
    public void Validate_Should_Have_Errors_When_Password_Is_Null()
    {
        var dto = new LoginUserDto
        {
            Username = "username",
            Password = null!
        };

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.Password);
    }

    [Fact]
    public void Validate_Should_Have_Errors_When_Password_Is_Empty()
    {
        var dto = new LoginUserDto
        { 
            Username = "username", 
            Password = string.Empty 
        };

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.Password);
    }

    [Fact]
    public void Validate_Should_Not_Have_Errors_When_Dto_Is_Valid()
    {
        var dto = new LoginUserDto
        { 
            Username = "username", 
            Password = "password" 
        };

        var result = _validator.TestValidate(dto);

        result.ShouldNotHaveValidationErrorFor(x => x.Username);
        result.ShouldNotHaveValidationErrorFor(x => x.Password);
    }
}