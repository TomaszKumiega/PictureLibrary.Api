using FluentValidation.TestHelper;
using MongoDB.Bson;
using PictureLibrary.Application.DtoValidators;
using PictureLibrary.Contracts;

namespace PictureLibrary.Application.Test.Validators.CreateImageUploadSession;

public class CreateImageUploadSessionDtoValidatorTest
{
    private readonly CreateImageUploadSessionDtoValidator _validator;

    public CreateImageUploadSessionDtoValidatorTest()
    {
        _validator = new CreateImageUploadSessionDtoValidator();
    }

    [Fact]
    public void Should_Have_Error_When_FileName_Is_Empty()
    {
        var dto = new CreateImageUploadSessionDto
        {
            FileName = string.Empty,
            FileLength = 123,
            LibraryId = ObjectId.GenerateNewId().ToString(),
            TagIds = []
        };

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.FileName);
    }

    [Fact]
    public void Should_Have_Error_When_FileLength_Is_Empty()
    {
        var dto = new CreateImageUploadSessionDto
        {
            FileName = "file.jpg",
            FileLength = 0,
            LibraryId = ObjectId.GenerateNewId().ToString(),
            TagIds = []
        };

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.FileLength);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Dto_Is_Empty()
    {
        var dto = new CreateImageUploadSessionDto
        {
            FileName = "file.jpg",
            FileLength = 123,
            LibraryId = ObjectId.GenerateNewId().ToString(),
            TagIds = []
        };

        var result = _validator.TestValidate(dto);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Should_Have_Error_When_LibraryId_Is_Empty()
    {
        var dto = new CreateImageUploadSessionDto
        {
            FileName = "file.jpg",
            FileLength = 123,
            LibraryId = string.Empty,
            TagIds = []
        };

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.LibraryId);
    }

    [Fact]
    public void Should_Have_Error_When_LibraryId_Is_Invalid_ObjectId()
    {
        var dto = new CreateImageUploadSessionDto
        {
            FileName = "file.jpg",
            FileLength = 123,
            LibraryId = "test",
            TagIds = []
        };

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.LibraryId);
    }
}