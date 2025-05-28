using FluentValidation.TestHelper;
using MongoDB.Bson;
using PictureLibrary.Application.Query;

namespace PictureLibrary.Application.Test.Validators.GetAllLibraries;

public class GetAllLibrariesQueryValidatorTest
{
    private readonly GetAllLibrariesQueryValidator _validator;

    public GetAllLibrariesQueryValidatorTest()
    {
        _validator = new GetAllLibrariesQueryValidator();
    }

    [Fact]
    public void Validate_Should_Have_Errors_When_UserId_Is_Empty()
    {
        var query = new GetAllLibrariesQuery(string.Empty);

        var result = _validator.TestValidate(query);

        result.ShouldHaveValidationErrorFor(x => x.UserId);
    }

    [Fact]
    public void Validate_Should_Not_Have_Errors_When_Query_Is_Valid()
    {
        var query = new GetAllLibrariesQuery(ObjectId.GenerateNewId().ToString());

        var result = _validator.TestValidate(query);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_Should_Have_Errors_When_UserId_Is_Null()
    {
        var query = new GetAllLibrariesQuery(null!);

        var result = _validator.TestValidate(query);

        result.ShouldHaveValidationErrorFor(x => x.UserId);
    }

    [Fact]
    public void Validate_Should_Have_Errors_When_UserId_Is_Invalid_ObjectId()
    {
        var query = new GetAllLibrariesQuery("test");

        var result = _validator.TestValidate(query);

        result.ShouldHaveValidationErrorFor(x => x.UserId);
    }
}