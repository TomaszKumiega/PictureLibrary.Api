using FluentValidation.TestHelper;
using PictureLibrary.Application.Command;
using PictureLibrary.Application.Command.RegsiterUser;
using PictureLibrary.Application.DtoValidators;
using PictureLibrary.Application.Test.Fakers;

namespace PictureLibrary.Application.Test.Validators.RegisterUser;

public class RegisterUserCommandValidatorTest
{
    private readonly RegisterUserCommandValidator _validator;

    public RegisterUserCommandValidatorTest()
    {
        _validator = new RegisterUserCommandValidator(new NewUserValidator());
    }

    [Fact]
    public void Validate_Should_Have_Errors_When_NewUser_Is_Null()
    {
        var command = new RegisterUserCommand(null!);

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.NewUser);
    }

    [Fact]
    public void Validate_Should_Not_Have_Errors_When_NewUser_Is_Valid()
    {
        var command = new RegisterUserCommand(new NewUserDtoFaker().Generate());

        var result = _validator.TestValidate(command);

        result.ShouldNotHaveAnyValidationErrors();
    }
}