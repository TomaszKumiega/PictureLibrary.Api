using FluentValidation.TestHelper;
using MongoDB.Bson;
using PictureLibrary.Application.Query;

namespace PictureLibrary.Application.Test.Validators.GetAllTags
{
    public class GetAllTagsQueryValidatorTest
    {
        private readonly GetAllTagsQueryValidator _validator;

        public GetAllTagsQueryValidatorTest()
        {
            _validator = new GetAllTagsQueryValidator();
        }

        [Fact]
        public void Validate_Should_Have_Errors_When_UserId_Is_Empty()
        {
            var query = new GetAllTagsQuery(string.Empty, ObjectId.GenerateNewId().ToString());

            var result = _validator.TestValidate(query);

            result.ShouldHaveValidationErrorFor(x => x.UserId);
        }

        [Fact]
        public void Validate_Should_Have_Errors_When_LibraryId_Is_Empty()
        {
            var query = new GetAllTagsQuery(ObjectId.GenerateNewId().ToString(), string.Empty);

            var result = _validator.TestValidate(query);

            result.ShouldHaveValidationErrorFor(x => x.LibraryId);
        }

        [Fact]
        public void Validate_Should_Not_Have_Errors_When_Query_Is_Valid()
        {
            var query = new GetAllTagsQuery(ObjectId.GenerateNewId().ToString(), ObjectId.GenerateNewId().ToString());

            var result = _validator.TestValidate(query);

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Validate_Should_Have_Errors_When_UserId_Is_Null()
        {
            var query = new GetAllTagsQuery(null, ObjectId.GenerateNewId().ToString());

            var result = _validator.TestValidate(query);

            result.ShouldHaveValidationErrorFor(x => x.UserId);
        }

        [Fact]
        public void Validate_Should_Have_Errors_When_LibraryId_Is_Null()
        {
            var query = new GetAllTagsQuery(ObjectId.GenerateNewId().ToString(), null);

            var result = _validator.TestValidate(query);

            result.ShouldHaveValidationErrorFor(x => x.LibraryId);
        }

        [Fact]
        public void Validate_Should_Have_Errors_When_UserId_Is_Invalid_ObjectId()
        {
            var query = new GetAllTagsQuery("test", ObjectId.GenerateNewId().ToString());

            var result = _validator.TestValidate(query);

            result.ShouldHaveValidationErrorFor(x => x.UserId);
        }

        [Fact]
        public void Validate_Should_Have_Errors_When_LibraryId_Is_Invalid_ObjectId()
        {
            var query = new GetAllTagsQuery(ObjectId.GenerateNewId().ToString(), "test");

            var result = _validator.TestValidate(query);

            result.ShouldHaveValidationErrorFor(x => x.LibraryId);
        }
    }
}
