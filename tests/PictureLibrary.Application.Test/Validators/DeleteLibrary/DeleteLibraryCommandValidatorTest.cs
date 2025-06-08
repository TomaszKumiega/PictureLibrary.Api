using FluentValidation.TestHelper;
using MongoDB.Bson;
using PictureLibrary.Application.Command;
using PictureLibrary.Application.Command.DeleteLibrary;

namespace PictureLibrary.Application.Test.Validators.DeleteLibrary;

public class DeleteLibraryCommandValidatorTest
{
    private readonly DeleteLibraryCommandValidator _validator;

    public DeleteLibraryCommandValidatorTest()
    {
        _validator = new DeleteLibraryCommandValidator();
    }

    [Fact]
    public void Validate_Should_Have_Errors_When_UserId_Is_Empty()
    {
        var command = new DeleteLibraryCommand(string.Empty, ObjectId.GenerateNewId().ToString());

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.UserId);
    }

    [Fact]
    public void Validate_Should_Have_Errors_When_LibraryId_Is_Empty()
    {
        var command = new DeleteLibraryCommand(ObjectId.GenerateNewId().ToString(), string.Empty);

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.LibraryId);
    }

    [Fact]
    public void Validate_Should_Not_Have_Errors_When_Command_Is_Valid()
    {
        var command = new DeleteLibraryCommand(ObjectId.GenerateNewId().ToString(), ObjectId.GenerateNewId().ToString());

        var result = _validator.TestValidate(command);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_Should_Have_Errors_When_UserId_Is_Null()
    {
        var command = new DeleteLibraryCommand(null!, ObjectId.GenerateNewId().ToString());

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.UserId);
    }

    [Fact]
    public void Validate_Should_Have_Errors_When_LibraryId_Is_Null()
    {
        var command = new DeleteLibraryCommand(ObjectId.GenerateNewId().ToString(), null!);

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.LibraryId);
    }

    [Fact]
    public void Validate_Should_Have_Errors_When_UserId_Is_Invalid_ObjectId()
    {
        var command = new DeleteLibraryCommand("test", ObjectId.GenerateNewId().ToString());

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.UserId);
    }

    [Fact]
    public void Validate_Should_Have_Errors_When_LibraryId_Is_Invalid_ObjectId()
    {
        var command = new DeleteLibraryCommand(ObjectId.GenerateNewId().ToString(), "test");

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.LibraryId);
    }
}