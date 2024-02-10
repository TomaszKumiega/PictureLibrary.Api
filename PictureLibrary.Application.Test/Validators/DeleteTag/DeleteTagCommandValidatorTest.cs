using FluentValidation.TestHelper;
using MongoDB.Bson;
using PictureLibrary.Application.Command;

namespace PictureLibrary.Application.Test.Validators.DeleteTag
{
    public class DeleteTagCommandValidatorTest
    {
        private readonly DeleteTagCommandValidator _validator;

        public DeleteTagCommandValidatorTest()
        {
            _validator = new DeleteTagCommandValidator();
        }

        [Fact]
        public void Validate_Should_Have_Errors_When_TagId_Is_Null()
        {
            var command = new DeleteTagCommand(ObjectId.GenerateNewId().ToString(), null);

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.TagId);
        }

        [Fact]
        public void Validate_Should_Have_Errors_When_TagId_Is_Empty()
        {
            var command = new DeleteTagCommand(ObjectId.GenerateNewId().ToString(), string.Empty);

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.TagId);
        }

        [Fact]
        public void Validate_Should_Have_Errors_When_UserId_Is_Null()
        {
            var command = new DeleteTagCommand(null, ObjectId.GenerateNewId().ToString());

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.UserId);
        }

        [Fact]
        public void Validate_Should_Have_Errors_When_UserId_Is_Empty()
        {
            var command = new DeleteTagCommand(string.Empty, ObjectId.GenerateNewId().ToString());

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.UserId);
        }

        [Fact]
        public void Validate_Should_Not_Have_Errors()
        {
            var command = new DeleteTagCommand(ObjectId.GenerateNewId().ToString(), ObjectId.GenerateNewId().ToString());

            var result = _validator.TestValidate(command);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
