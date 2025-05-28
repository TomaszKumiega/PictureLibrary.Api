using FluentValidation.TestHelper;
using MongoDB.Bson;
using PictureLibrary.Application.Command;

namespace PictureLibrary.Application.Test.Validators.DeleteUser;

public class DeleteUserCommandValidatorTest
{
    private readonly DeleteUserCommandValidator _validator;

    public DeleteUserCommandValidatorTest()
    {
        _validator = new DeleteUserCommandValidator();
    }

    [Fact]
    public void Validate_Should_Have_Errors_When_UserId_Is_Null()
    {
        var command = new DeleteUserCommand(null!);

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.UserId);
    }

    [Fact]
    public void Validate_Should_Have_Errors_When_UserId_Is_Empty()
    {
        var command = new DeleteUserCommand(string.Empty);

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.UserId);
    }

    [Fact]
    public void Validate_Should_Have_Errors_When_UserId_Is_Not_ObjectId()
    {
        var command = new DeleteUserCommand("not valid object id");

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.UserId);
    }

    [Fact]
    public void Validate_Should_Not_Have_Errors_When_UserId_Is_Valid()
    {
        var command = new DeleteUserCommand(ObjectId.GenerateNewId().ToString());

        var result = _validator.TestValidate(command);

        result.ShouldNotHaveValidationErrorFor(x => x.UserId);
    }
}