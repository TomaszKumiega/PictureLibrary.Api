using FluentValidation.TestHelper;
using MongoDB.Bson;
using PictureLibrary.Application.Command;
using PictureLibrary.Application.Command.UpdateTag;
using PictureLibrary.Application.DtoValidators;
using PictureLibrary.Application.Test.Fakers;

namespace PictureLibrary.Application.Test.Validators.UpdateTag;

public class UpdateTagCommandValidatorTest
{
    private readonly UpdateTagCommandValidator _validator;

    public UpdateTagCommandValidatorTest()
    {
        _validator = new UpdateTagCommandValidator(new UpdateTagValidator());
    }

    [Fact]
    public void Validate_Should_Have_Errors_When_UserId_Is_Null()
    {
        var command = new UpdateTagCommand(null!, ObjectId.GenerateNewId().ToString(), new UpdateTagDtoFaker().Generate());

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.UserId);
    }

    [Fact]
    public void Validate_Should_Have_Errors_When_UserId_Is_Empty()
    {
        var command = new UpdateTagCommand(string.Empty, ObjectId.GenerateNewId().ToString(), new UpdateTagDtoFaker().Generate());

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.UserId);
    }

    [Fact]
    public void Validate_Should_Have_Errors_When_TagId_Is_Null()
    {
        var command = new UpdateTagCommand(ObjectId.GenerateNewId().ToString(), null!, new UpdateTagDtoFaker().Generate());

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.TagId);
    }

    [Fact]
    public void Validate_Should_Have_Errors_When_TagId_Is_Empty()
    {
        var command = new UpdateTagCommand(ObjectId.GenerateNewId().ToString(), string.Empty, new UpdateTagDtoFaker().Generate());

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.TagId);
    }

    [Fact]
    public void Validate_Should_Have_Errors_When_UpdateTagDto_Is_Null()
    {
        var command = new UpdateTagCommand(ObjectId.GenerateNewId().ToString(), ObjectId.GenerateNewId().ToString(), null!);

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.UpdateTagDto);
    }

    [Fact]
    public void Validate_Should_Not_Have_Errors_When_Command_Is_Valid()
    {
        var command = new UpdateTagCommand(ObjectId.GenerateNewId().ToString(), ObjectId.GenerateNewId().ToString(), new UpdateTagDtoFaker().Generate());

        var result = _validator.TestValidate(command);

        result.ShouldNotHaveAnyValidationErrors();
    }
}