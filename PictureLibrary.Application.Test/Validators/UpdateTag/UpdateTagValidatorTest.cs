using FluentValidation.TestHelper;
using PictureLibrary.Application.DtoValidators;
using PictureLibrary.Contracts;

namespace PictureLibrary.Application.Test.Validators.UpdateTag
{
    public class UpdateTagValidatorTest
    {
        private readonly UpdateTagValidator _validator;

        public UpdateTagValidatorTest()
        {
            _validator = new UpdateTagValidator();
        }

        [Fact]
        public void Validate_Should_Have_Errors_When_Name_Is_Null()
        {
            var tag = new UpdateTagDto()
            {
                Name = null,
                ColorHex = "#FFFFFF"
            };

            var result = _validator.TestValidate(tag);

            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public void Validate_Should_Have_Errors_When_Name_Is_Empty()
        {
            var tag = new UpdateTagDto()
            {
                Name = string.Empty,
                ColorHex = "#FFFFFF"
            };

            var result = _validator.TestValidate(tag);

            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public void Validate_Should_Have_Errors_When_ColorHex_Is_Null()
        {
            var tag = new UpdateTagDto()
            {
                Name = "Tag",
                ColorHex = null
            };

            var result = _validator.TestValidate(tag);

            result.ShouldHaveValidationErrorFor(x => x.ColorHex);
        }

        [Fact]
        public void Validate_Should_Have_Errors_When_ColorHex_Is_Empty()
        {
            var tag = new UpdateTagDto()
            {
                Name = "Tag",
                ColorHex = string.Empty
            };

            var result = _validator.TestValidate(tag);

            result.ShouldHaveValidationErrorFor(x => x.ColorHex);
        }

        [Fact]
        public void Validate_Should_Have_Errors_When_ColorHex_Is_Not_Color()
        {
            var tag = new UpdateTagDto()
            {
                Name = "Tag",
                ColorHex = "NotColor"
            };

            var result = _validator.TestValidate(tag);

            result.ShouldHaveValidationErrorFor(x => x.ColorHex);
        }

        [Fact]
        public void Validate_Should_Not_Have_Errors_When_Tag_Is_Valid()
        {
            var tag = new UpdateTagDto()
            {
                Name = "Tag",
                ColorHex = "#FFFFFF"
            };

            var result = _validator.TestValidate(tag);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
