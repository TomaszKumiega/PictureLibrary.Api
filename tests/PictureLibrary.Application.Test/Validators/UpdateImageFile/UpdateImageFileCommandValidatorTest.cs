using FluentValidation.TestHelper;
using MongoDB.Bson;
using PictureLibrary.Application.Command;

namespace PictureLibrary.Application.Test.Validators.UpdateImageFile;

public class UpdateImageFileCommandValidatorTest
{
    private readonly UpdateImageFileCommandValidator _validator;

    public UpdateImageFileCommandValidatorTest()
    {
        _validator = new UpdateImageFileCommandValidator(new UpdateImageFileDtoValidator());
    }

    [Fact]
    public void Validate_Should_Have_Errors_When_UserId_Is_Empty()
    {
        var command = new UpdateImageFileCommand(string.Empty, ObjectId.GenerateNewId().ToString(), new UpdateImageFileDtoFaker().Generate());

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.UserId);
    }

    [Fact]
    public void Validate_Should_Have_Errors_When_ImageFileId_Is_Empty()
    {
        var command = new UpdateImageFileCommand(ObjectId.GenerateNewId().ToString(), string.Empty, new UpdateImageFileDtoFaker().Generate());

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.ImageFileId);
    }

    [Fact]
    public void Validate_Should_Have_Errors_When_Dto_Is_Empty()
    {
        var command = new UpdateImageFileCommand(ObjectId.GenerateNewId().ToString(), ObjectId.GenerateNewId().ToString(), null!);

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Dto);
    }

    [Fact]
    public void Validate_Should_Not_Have_Errors_When_Command_Is_Valid()
    {
        var command = new UpdateImageFileCommand(ObjectId.GenerateNewId().ToString(), ObjectId.GenerateNewId().ToString(), new UpdateImageFileDtoFaker().Generate());

        var result = _validator.TestValidate(command);

        result.ShouldNotHaveAnyValidationErrors();
    }
}