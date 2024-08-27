using FluentValidation.TestHelper;
using MongoDB.Bson;
using PictureLibrary.Application.Command;
using PictureLibrary.Application.DtoValidators;
using PictureLibrary.Application.Test.Fakers;

namespace PictureLibrary.Application.Test.Validators.UpdateLibrary
{
    public class UpdateLibraryCommandValidatorTest
    {
        private readonly UpdateLibraryCommandValidator _validator;

        public UpdateLibraryCommandValidatorTest()
        {
            _validator = new UpdateLibraryCommandValidator(new UpdateLibraryValidator());
        }

        [Fact]
        public void Validate_Should_Have_Errors_When_UserId_Is_Empty()
        {
            var command = new UpdateLibraryCommand(string.Empty, ObjectId.GenerateNewId().ToString(), new UpdateLibraryDtoFaker().Generate());

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.UserId);
        }

        [Fact]
        public void Validate_Should_Have_Errors_When_LibraryId_Is_Empty()
        {
            var command = new UpdateLibraryCommand(ObjectId.GenerateNewId().ToString(), string.Empty, new UpdateLibraryDtoFaker().Generate());

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.LibraryId);
        }

        [Fact]
        public void Validate_Should_Have_Errors_When_Library_Is_Empty()
        {
            var command = new UpdateLibraryCommand(ObjectId.GenerateNewId().ToString(), ObjectId.GenerateNewId().ToString(), null!);

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Library);
        }

        [Fact]
        public void Validate_Should_Not_Have_Errors_When_Command_Is_Valid()
        {
            var command = new UpdateLibraryCommand(ObjectId.GenerateNewId().ToString(), ObjectId.GenerateNewId().ToString(), new UpdateLibraryDtoFaker().Generate());

            var result = _validator.TestValidate(command);

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Validate_Should_Have_Errors_When_UserId_Is_Null()
        {
            var command = new UpdateLibraryCommand(null!, ObjectId.GenerateNewId().ToString(), new UpdateLibraryDtoFaker().Generate());

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.UserId);
        }

        [Fact]
        public void Validate_Should_Have_Errors_When_LibraryId_Is_Null()
        {
            var command = new UpdateLibraryCommand(ObjectId.GenerateNewId().ToString(), null!, new UpdateLibraryDtoFaker().Generate());

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.LibraryId);
        }

        [Fact]
        public void Validate_Should_Have_Errors_When_UserId_Is_Not_ObjectId()
        {
            var command = new UpdateLibraryCommand("test", ObjectId.GenerateNewId().ToString(), new UpdateLibraryDtoFaker().Generate());

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.UserId);
        }

        [Fact]
        public void Validate_Should_Have_Errors_When_LibraryId_Is_Not_ObjectId()
        {
            var command = new UpdateLibraryCommand(ObjectId.GenerateNewId().ToString(), "test", new UpdateLibraryDtoFaker().Generate());

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.LibraryId);
        }
    }
}
