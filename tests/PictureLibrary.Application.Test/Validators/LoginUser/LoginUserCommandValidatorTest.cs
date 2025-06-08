using FluentValidation.TestHelper;
using PictureLibrary.Application.Command;
using PictureLibrary.Application.Command.LoginUser;
using PictureLibrary.Application.DtoValidators;
using PictureLibrary.Application.Test.Fakers;

namespace PictureLibrary.Application.Test.Validators.LoginUser;

public class LoginUserCommandValidatorTest
{
    private readonly LoginUserCommandValidator _validator;

    public LoginUserCommandValidatorTest()
    {
        _validator = new LoginUserCommandValidator(new LoginUserDtoValidator());
    }

    [Fact]
    public void Validate_Should_Have_Errors_When_LoginUserDto_Is_Null()
    {
        var command = new LoginUserCommand(null!);

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.LoginDto);
    }

    [Fact]
    public void Validate_Should_Not_Have_Errors_When_LoginUserDto_Is_Valid()
    {
        var command = new LoginUserCommand(new LoginUserDtoFaker().Generate());

        var result = _validator.TestValidate(command);

        result.ShouldNotHaveValidationErrorFor(x => x.LoginDto);
    }
}