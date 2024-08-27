using FluentValidation.TestHelper;
using MongoDB.Bson;
using PictureLibrary.Application.Command;
using PictureLibrary.Application.DtoValidators;
using PictureLibrary.Application.Test.Fakers;

namespace PictureLibrary.Application.Test.Validators.CreateImageUploadSession
{
    public class CreateImageUploadSessionCommandValidatorTest
    {
        private readonly CreateImageUploadSessionCommandValidator _validator;

        public CreateImageUploadSessionCommandValidatorTest()
        {
            _validator = new CreateImageUploadSessionCommandValidator(new CreateImageUploadSessionDtoValidator());
        }

        [Fact]
        public void Should_Have_Error_When_UserId_Is_Empty()
        {
            var dto = new CreateImageUploadSessionDtoFaker().Generate();

            var command = new CreateImageUploadSessionCommand(string.Empty, dto);

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.UserId);
        }

        [Fact]
        public void Should_Have_Error_When_UserId_Is_Not_ObjectId()
        {
            var dto = new CreateImageUploadSessionDtoFaker().Generate();

            var command = new CreateImageUploadSessionCommand("notObjectId", dto);

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.UserId);
        }

        [Fact]
        public void Should_Have_Error_When_CreateUploadSessionDto_Is_Null()
        {
            var command = new CreateImageUploadSessionCommand(ObjectId.GenerateNewId().ToString(), null!);

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.CreateUploadSessionDto);
        }

        [Fact]
        public void Should_Not_Have_Error_When_Command_Is_Valid()
        {
            var dto = new CreateImageUploadSessionDtoFaker().Generate();

            var command = new CreateImageUploadSessionCommand(ObjectId.GenerateNewId().ToString(), dto);

            var result = _validator.TestValidate(command);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
