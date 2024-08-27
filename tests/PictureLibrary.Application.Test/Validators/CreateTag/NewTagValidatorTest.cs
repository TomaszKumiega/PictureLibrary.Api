using FluentValidation.TestHelper;
using PictureLibrary.Application.DtoValidators;
using PictureLibrary.Contracts;

namespace PictureLibrary.Application.Test.Validators.CreateTag
{
    public class NewTagValidatorTest
    {
        private readonly NewTagValidator _validator;

        public NewTagValidatorTest()
        {
            _validator = new NewTagValidator();
        }

        [Fact]
        public void Validate_Should_Have_Errors_When_Name_Is_Null()
        {
            var tag = new NewTagDto()
            {
                Name = null!,
                ColorHex = "#FFFFFF"
            };

            var result = _validator.TestValidate(tag);

            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public void Validate_Should_Have_Errors_When_Name_Is_Empty()
        {
            var tag = new NewTagDto()
            {
                Name = "",
                ColorHex = "#FFFFFF"
            };

            var result = _validator.TestValidate(tag);

            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public void Validate_Should_Have_Errors_When_ColorHex_Is_Null()
        {
            var tag = new NewTagDto()
            {
                Name = "Tag",
                ColorHex = null!
            };

            var result = _validator.TestValidate(tag);

            result.ShouldHaveValidationErrorFor(x => x.ColorHex);
        }

        [Fact]
        public void Validate_Should_Have_Errors_When_ColorHex_Is_Empty()
        {
            var tag = new NewTagDto()
            {
                Name = "Tag",
                ColorHex = ""
            };

            var result = _validator.TestValidate(tag);

            result.ShouldHaveValidationErrorFor(x => x.ColorHex);
        }

        [Fact]
        public void Validate_Should_Have_Errors_When_ColorHex_Is_Not_Color()
        {
            var tag = new NewTagDto()
            {
                Name = "Tag",
                ColorHex = "NotColor"
            };

            var result = _validator.TestValidate(tag);

            result.ShouldHaveValidationErrorFor(x => x.ColorHex);
        }

        [Fact]
        public void Validate_Should_Not_Have_Errors_When_Dto_Is_Valid()
        {
            var tag = new NewTagDto()
            {
                Name = "Tag",
                ColorHex = "#FFFFFF"
            };

            var result = _validator.TestValidate(tag);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
