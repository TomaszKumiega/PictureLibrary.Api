using FluentValidation.TestHelper;
using MongoDB.Bson;
using PictureLibrary.Application.DtoValidators;
using PictureLibrary.Contracts;

namespace PictureLibrary.Application.Test.Validators.UpdateImageFile;

public class UpdateImageFileDtoValidatorTest
{
    private readonly UpdateImageFileDtoValidator _validator;

    public UpdateImageFileDtoValidatorTest()
    {
        _validator = new UpdateImageFileDtoValidator();
    }

    [Fact]
    public void Should_Have_Errors_When_FileName_Is_Empty()
    {
        var dto = new UpdateImageFileDto() { FileName = string.Empty };

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.FileName);
    }

    [Fact]
    public void Should_Have_Errors_When_LibraryId_Is_Empty()
    {
        var dto = new UpdateImageFileDto() { LibraryId = string.Empty };

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.LibraryId);
    }

    [Fact]
    public void Should_Have_Errors_When_TagIds_Contains_Empty_Strings()
    {
        var dto = new UpdateImageFileDto() { TagIds = [string.Empty] };

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.TagIds);
    }

    [Fact]
    public void Should_Have_Errors_When_LibraryId_Is_Not_Valid_ObjectId()
    {
        var dto = new UpdateImageFileDto() { LibraryId = "test" };

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.LibraryId);
    }

    [Fact]
    public void Should_Have_Errors_When_TagIds_Contains_Not_Valid_ObjectIds()
    {
        var dto = new UpdateImageFileDto() { TagIds = ["test"] };

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.TagIds);
    }

    [Fact]
    public void Should_Not_Have_Errors_When_Dto_Is_Valid()
    {
        var dto = new UpdateImageFileDto() 
        { 
            FileName = "test",
            LibraryId = ObjectId.GenerateNewId().ToString(),
            TagIds = [ObjectId.GenerateNewId().ToString()]
        };

        var result = _validator.TestValidate(dto);

        result.ShouldNotHaveAnyValidationErrors();
    }
}