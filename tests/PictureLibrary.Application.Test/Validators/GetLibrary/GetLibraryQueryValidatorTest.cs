using FluentValidation.TestHelper;
using MongoDB.Bson;
using PictureLibrary.Application.Query;

namespace PictureLibrary.Application.Test.Validators.GetLibrary;

public class GetLibraryQueryValidatorTest
{
    private readonly GetLibraryQueryValidator _validator;

    public GetLibraryQueryValidatorTest()
    {
        _validator = new GetLibraryQueryValidator();
    }

    [Fact]
    public void Validate_Should_Have_Errors_When_UserId_Is_Empty()
    {
        var query = new GetLibraryQuery(string.Empty, ObjectId.GenerateNewId().ToString());

        var result = _validator.TestValidate(query);

        result.ShouldHaveValidationErrorFor(x => x.UserId);
    }

    [Fact]
    public void Validate_Should_Have_Errors_When_LibraryId_Is_Empty()
    {
        var query = new GetLibraryQuery(ObjectId.GenerateNewId().ToString(), string.Empty);

        var result = _validator.TestValidate(query);

        result.ShouldHaveValidationErrorFor(x => x.LibraryId);
    }

    [Fact]
    public void Validate_Should_Not_Have_Errors_When_Query_Is_Valid()
    {
        var query = new GetLibraryQuery(ObjectId.GenerateNewId().ToString(), ObjectId.GenerateNewId().ToString());

        var result = _validator.TestValidate(query);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_Should_Have_Errors_When_UserId_Is_Null()
    {
        var query = new GetLibraryQuery(null!, ObjectId.GenerateNewId().ToString());

        var result = _validator.TestValidate(query);

        result.ShouldHaveValidationErrorFor(x => x.UserId);
    }

    [Fact]
    public void Validate_Should_Have_Errors_When_LibraryId_Is_Null()
    {
        var query = new GetLibraryQuery(ObjectId.GenerateNewId().ToString(), null!);

        var result = _validator.TestValidate(query);

        result.ShouldHaveValidationErrorFor(x => x.LibraryId);
    }

    [Fact]
    public void Validate_Should_Have_Errors_When_UserId_Is_Not_ObjectId()
    {
        var query = new GetLibraryQuery("test", ObjectId.GenerateNewId().ToString());

        var result = _validator.TestValidate(query);

        result.ShouldHaveValidationErrorFor(x => x.UserId);
    }

    [Fact]
    public void Validate_Should_Have_Errors_When_LibraryId_Is_Not_ObjectId()
    {
        var query = new GetLibraryQuery(ObjectId.GenerateNewId().ToString(), "test");

        var result = _validator.TestValidate(query);

        result.ShouldHaveValidationErrorFor(x => x.LibraryId);
    }

    [Fact]
    public void Validate_Should_Have_Errors_When_UserId_Is_WhiteSpace()
    {
        var query = new GetLibraryQuery(" ", ObjectId.GenerateNewId().ToString());

        var result = _validator.TestValidate(query);

        result.ShouldHaveValidationErrorFor(x => x.UserId);
    }

    [Fact]
    public void Validate_Should_Have_Errors_When_LibraryId_Is_WhiteSpace()
    {
        var query = new GetLibraryQuery(ObjectId.GenerateNewId().ToString(), " ");

        var result = _validator.TestValidate(query);

        result.ShouldHaveValidationErrorFor(x => x.LibraryId);
    }
}