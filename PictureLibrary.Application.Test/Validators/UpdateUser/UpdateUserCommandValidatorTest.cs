using FluentValidation.TestHelper;
using PictureLibrary.Application.Command;
using PictureLibrary.Application.DtoValidators;
using PictureLibrary.Application.Test.Fakers;

namespace PictureLibrary.Application.Test.Validators.UpdateUser
{
    public class UpdateUserCommandValidatorTest
    {
        private readonly UpdateUserCommandValidator _validator;

        public UpdateUserCommandValidatorTest()
        {
            _validator = new UpdateUserCommandValidator(new UpdateUserDtoValidator());
        }

        [Fact]
        public void Validate_Should_Have_Errors_When_UserDto_Is_Null()
        {
            var command = new UpdateUserCommand(null);

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.UserDto);
        }

        [Fact]
        public void Validate_Should_Not_Have_Errors_When_UserDto_Is_Not_Null()
        {
            var command = new UpdateUserCommand(new UpdateUserDtoFaker().Generate());

            var result = _validator.TestValidate(command);

            result.ShouldNotHaveValidationErrorFor(x => x.UserDto);
        }
    }
}
