using FluentValidation.TestHelper;
using MongoDB.Bson;
using PictureLibrary.Application.Command;
using PictureLibrary.Application.Command.DeleteImageFile;

namespace PictureLibrary.Application.Test.Validators;

public class DeleteImageFileCommandValidatorTest
{
    private readonly DeleteImageFileCommandValidator _validator;

    public DeleteImageFileCommandValidatorTest()
    {
        _validator = new DeleteImageFileCommandValidator();
    }

    [Fact]
    public void Validate_Should_Have_Errors_When_UserId_Is_Empty()
    {
        var command = new DeleteImageFileCommand(string.Empty, ObjectId.GenerateNewId().ToString());

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.UserId);
    }

    [Fact]
    public void Validate_Should_Have_Errors_When_ImageFileId_Is_Empty()
    {
        var command = new DeleteImageFileCommand(ObjectId.GenerateNewId().ToString(), string.Empty);

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.ImageFileId);
    }

    [Fact]
    public void Validate_Should_Not_Have_Errors_When_Command_Is_Valid()
    {
        var command = new DeleteImageFileCommand(ObjectId.GenerateNewId().ToString(), ObjectId.GenerateNewId().ToString());

        var result = _validator.TestValidate(command);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_Should_Have_Errors_When_UserId_Is_Null()
    {
        var command = new DeleteImageFileCommand(null!, ObjectId.GenerateNewId().ToString());

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.UserId);
    }

    [Fact]
    public void Validate_Should_Have_Errors_When_ImageFileId_Is_Null()
    {
        var command = new DeleteImageFileCommand(ObjectId.GenerateNewId().ToString(), null!);

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.ImageFileId);
    }

}