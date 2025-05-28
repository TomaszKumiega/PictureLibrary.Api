using FluentValidation.TestHelper;
using MongoDB.Bson;
using PictureLibrary.Application.Command;
using PictureLibrary.Application.DtoValidators;
using PictureLibrary.Application.Test.Fakers;

namespace PictureLibrary.Application.Test.Validators.UpdateUser;

public class UpdateUserCommandValidatorTest
{
    private readonly UpdateUserCommandValidator _validator;

    public UpdateUserCommandValidatorTest()
    {
        _validator = new UpdateUserCommandValidator(new UpdateUserDtoValidator());
    }

    [Fact]
    public void Validate_Should_Have_Errors_When_UserDto_Is_Null()
    {
        var command = new UpdateUserCommand(ObjectId.GenerateNewId().ToString(), null!);

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.UserDto);
    }

    [Fact]
    public void Validate_Should_Have_Errors_When_UserId_Is_Null()
    {
        var command = new UpdateUserCommand(null!, new UpdateUserDtoFaker().Generate());

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.UserId);
    }

    [Fact]
    public void Validate_Should_Have_Errors_When_UserId_Is_Empty()
    {
        var command = new UpdateUserCommand(string.Empty, new UpdateUserDtoFaker().Generate());

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.UserId);
    }

    [Fact]
    public void Validate_Should_Have_Errors_When_UserId_Is_Not_ObjectId()
    {
        var command = new UpdateUserCommand("not valid object id", new UpdateUserDtoFaker().Generate());

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.UserId);
    }

    [Fact]
    public void Validate_Should_Not_Have_Errors_When_Command_Is_Valid()
    {
        var command = new UpdateUserCommand(ObjectId.GenerateNewId().ToString(), new UpdateUserDtoFaker().Generate());

        var result = _validator.TestValidate(command);

        result.ShouldNotHaveValidationErrorFor(x => x.UserDto);
    }
}