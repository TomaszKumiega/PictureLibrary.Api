using FluentValidation.TestHelper;
using MongoDB.Bson;
using PictureLibrary.Application.Command;
using PictureLibrary.Application.DtoValidators;
using PictureLibrary.Application.Test.Fakers;

namespace PictureLibrary.Application.Test.Validators.CreateTag
{
    public class CreateTagCommandValidatorTest
    {
        private readonly CreateTagCommandValidator _validator;

        public CreateTagCommandValidatorTest()
        {
            _validator = new CreateTagCommandValidator(new NewTagValidator());
        }

        [Fact]
        public void Validate_Should_Have_Errors_When_LibraryId_Is_Null()
        {
            var command = new CreateTagCommand(ObjectId.GenerateNewId().ToString(), null!, new NewTagDtoFaker().Generate());

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.LibraryId);
        }

        [Fact]
        public void Validate_Should_Have_Errors_When_LibraryId_Is_Empty()
        {
            var command = new CreateTagCommand(ObjectId.GenerateNewId().ToString(), string.Empty, new NewTagDtoFaker().Generate());

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.LibraryId);
        }

        [Fact]
        public void Validate_Should_Have_Errors_When_UserId_Is_Null()
        {
            var command = new CreateTagCommand(null!, ObjectId.GenerateNewId().ToString(), new NewTagDtoFaker().Generate());

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.UserId);
        }

        [Fact]
        public void Validate_Should_Have_Errors_When_UserId_Is_Empty()
        {
            var command = new CreateTagCommand(string.Empty, ObjectId.GenerateNewId().ToString(), new NewTagDtoFaker().Generate());

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.UserId);
        }

        [Fact]
        public void Validate_Should_Have_Errors_When_NewTagDto_Is_Null()
        {
            var command = new CreateTagCommand(ObjectId.GenerateNewId().ToString(), ObjectId.GenerateNewId().ToString(), null!);

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.NewTagDto);
        }

        [Fact]
        public void Validate_Should_Not_Have_Errors_When_Command_Is_Valid()
        {
            var command = new CreateTagCommand(ObjectId.GenerateNewId().ToString(), ObjectId.GenerateNewId().ToString(), new NewTagDtoFaker().Generate());

            var result = _validator.TestValidate(command);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
