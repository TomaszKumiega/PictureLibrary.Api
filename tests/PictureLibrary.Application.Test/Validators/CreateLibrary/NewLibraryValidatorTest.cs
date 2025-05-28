using FluentValidation.TestHelper;
using PictureLibrary.Application.DtoValidators;
using PictureLibrary.Contracts.Library;

namespace PictureLibrary.Application.Test.Validators.CreateLibrary;

public class NewLibraryValidatorTest
{
    private readonly NewLibraryValidator _validator;

    public NewLibraryValidatorTest()
    {
        _validator = new NewLibraryValidator();
    }

    [Fact]
    public void Validate_Have_Errors_When_Name_Is_Empty()
    {
        var newLibrary = new NewLibraryDto()
        {
            Name = string.Empty
        };
        
        var result = _validator.TestValidate(newLibrary);
        
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Validate_Have_Errors_When_Name_Is_Null()
    {
        var newLibrary = new NewLibraryDto()
        {
            Name = null!
        };
        
        var result = _validator.TestValidate(newLibrary);
        
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Validate_Have_Errors_When_Name_Is_Whitespace()
    {
        var newLibrary = new NewLibraryDto()
        {
            Name = " "
        };
        
        var result = _validator.TestValidate(newLibrary);
        
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Validate_Doesnt_Have_Errors_When_Name_Is_Valid()
    {
        var newLibrary = new NewLibraryDto()
        {
            Name = "Name"
        };
        
        var result = _validator.TestValidate(newLibrary);
        
        result.ShouldNotHaveValidationErrorFor(x => x.Name);
    }   
}