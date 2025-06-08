using FluentValidation;
using MongoDB.Bson;
using PictureLibrary.Application.DtoValidators;

namespace PictureLibrary.Application.Command.UpdateImageFile;

public class UpdateImageFileCommandValidator : AbstractValidator<UpdateImageFileCommand>
{
    public UpdateImageFileCommandValidator(UpdateImageFileDtoValidator dtoValidator)
    {
        RuleFor(x => x.UserId).NotEmpty().Must(userId => ObjectId.TryParse(userId, out _));
        RuleFor(x => x.ImageFileId).NotEmpty().Must(ImageFileId => ObjectId.TryParse(ImageFileId, out _));
        RuleFor(x => x.Dto).NotEmpty().SetValidator(dtoValidator);
    }
}