using FluentValidation.TestHelper;
using PictureLibrary.Application.DtoValidators;
using PictureLibrary.Contracts;

namespace PictureLibrary.Application.Test.Validators.UpdateLibrary
{
    public class UpdateLibraryValidatorTest
    {
        private readonly UpdateLibraryValidator _validator;

        public UpdateLibraryValidatorTest()
        {
            _validator = new UpdateLibraryValidator();
        }

        [Fact]
        public void Validate_Should_Have_Errors_When_Library_Name_Is_Null()
        {
            var dto = new UpdateLibraryDto() { Name = null! };

            var result = _validator.TestValidate(dto);

            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public void Validate_Should_Have_Errors_When_Library_Name_Is_Empty()
        {
            var dto = new UpdateLibraryDto() { Name = string.Empty };

            var result = _validator.TestValidate(dto);

            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public void Validate_Should_Have_Errors_When_Library_Name_Is_Whitespace()
        {
            var dto = new UpdateLibraryDto() { Name = " " };

            var result = _validator.TestValidate(dto);

            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public void Validate_Should_Not_Have_Errors_When_Dto_Is_Valid()
        {
            var dto = new UpdateLibraryDto() 
            { 
                Name = "test" 
            };

            var result = _validator.TestValidate(dto);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
