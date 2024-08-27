using FluentValidation.TestHelper;
using PictureLibrary.Application.DtoValidators;
using PictureLibrary.Contracts;

namespace PictureLibrary.Application.Test.Validators.RegisterUser
{
    public class NewUserValidatorTest
    {
        private readonly NewUserValidator _validator;

        public NewUserValidatorTest()
        {
            _validator = new NewUserValidator();
        }

        [Fact]
        public void Validate_Should_Have_Validation_Errors_When_Username_Is_Null()
        {
            var dto = new NewUserDto()
            {
                Username = null!,
                Password = "password",
                Email = "email@email.com"
            };

            var result = _validator.TestValidate(dto);

            result.ShouldHaveValidationErrorFor(x => x.Username);
        }

        [Fact]
        public void Validate_Should_Have_Validation_Errors_When_Username_Is_Empty()
        {
            var dto = new NewUserDto()
            {
                Username = string.Empty,
                Password = "password",
                Email = "email"
            };

            var result = _validator.TestValidate(dto);

            result.ShouldHaveValidationErrorFor(x => x.Username);
        }

        [Fact]
        public void Validate_Should_Have_Validation_Errors_When_Password_Is_Null()
        {
            var dto = new NewUserDto()
            {
                Username = "username",
                Password = null!,
                Email = "email"
            };

            var result = _validator.TestValidate(dto);

            result.ShouldHaveValidationErrorFor(x => x.Password);
        }

        [Fact]
        public void Validate_Should_Have_Validation_Errors_When_Password_Is_Empty()
        {
            var dto = new NewUserDto()
            {
                Username = "username",
                Password = string.Empty,
                Email = "email"
            };

            var result = _validator.TestValidate(dto);

            result.ShouldHaveValidationErrorFor(x => x.Password);
        }

        [Fact]
        public void Validate_Should_Not_Have_Validation_Errors_When_Email_Is_Null()
        {
            var dto = new NewUserDto()
            {
                Username = "username",
                Password = "password",
                Email = null
            };

            var result = _validator.TestValidate(dto);

            result.ShouldNotHaveValidationErrorFor(x => x.Email);
        }

        [Fact]
        public void Validate_Should_Have_Validation_Errors_When_Password_Lenght_Is_Less_Than_7()
        {
            var dto = new NewUserDto()
            {
                Username = "username",
                Password = "passwo",
                Email = "email"
            };

            var result = _validator.TestValidate(dto);

            result.ShouldHaveValidationErrorFor(x => x.Password);
        }

        [Fact]
        public void Validate_Should_Not_Have_Validation_Errors_When_Dto_Is_Valid()
        {
            var dto = new NewUserDto()
            {
                Username = "username",
                Password = "password",
                Email = "email@info.com"
            };

            var result = _validator.TestValidate(dto);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
