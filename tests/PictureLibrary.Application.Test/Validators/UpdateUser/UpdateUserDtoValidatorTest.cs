using FluentValidation.TestHelper;
using PictureLibrary.Application.DtoValidators;
using PictureLibrary.Application.Test.Fakers;
using PictureLibrary.Contracts;

namespace PictureLibrary.Application.Test.Validators.UpdateUser
{
    public class UpdateUserDtoValidatorTest
    {
        private readonly UpdateUserDtoValidator _validator;

        public UpdateUserDtoValidatorTest()
        {
            _validator = new UpdateUserDtoValidator();
        }

        [Fact]
        public void Validate_Should_Have_Errors_When_Username_Is_Null()
        {
            var user = new UpdateUserDto()
            {
                Username = null!,
                Email = "email@example.com"
            };

            var result = _validator.TestValidate(user);

            result.ShouldHaveValidationErrorFor(x => x.Username);
        }

        [Fact]
        public void Validate_Should_Have_Errors_When_Username_Is_Empty()
        {
            var user = new UpdateUserDto()
            {
                Username = string.Empty,
                Email = "email@example.com"
            };

            var result = _validator.TestValidate(user);

            result.ShouldHaveValidationErrorFor(x => x.Username);
        }

        [Fact]
        public void Validate_Should_Have_Errors_When_Email_Is_Not_Valid()
        {
            var user = new UpdateUserDto()
            {
                Username = "Username",
                Email = "email"
            };

            var result = _validator.TestValidate(user);

            result.ShouldHaveValidationErrorFor(x => x.Email);
        }

        [Fact]
        public void Validate_Should_Not_Have_Errors_When_Email_Is_Null()
        {
            var user = new UpdateUserDto()
            {
                Username = "Username",
                Email = null
            };

            var result = _validator.TestValidate(user);

            result.ShouldNotHaveValidationErrorFor(x => x.Email);
        }

        [Fact]
        public void Validate_Should_Not_Have_Errors_When_Dto_Is_Valid()
        {
            var user = new UpdateUserDtoFaker().Generate();

            var result = _validator.TestValidate(user);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
