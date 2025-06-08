using FluentValidation;
using MongoDB.Bson;
using PictureLibrary.Application.DtoValidators;

namespace PictureLibrary.Application.Command.CreateTag;

public class CreateTagCommandValidator : AbstractValidator<CreateTagCommand>
{
    public CreateTagCommandValidator(NewTagValidator dtoValidator)
    {
        RuleFor(x => x.UserId).NotEmpty().Must(userId => ObjectId.TryParse(userId, out _));
        RuleFor(x => x.LibraryId).NotEmpty().Must(libraryId => ObjectId.TryParse(libraryId, out _));
        RuleFor(x => x.NewTagDto).NotNull().SetValidator(dtoValidator);
    }
}