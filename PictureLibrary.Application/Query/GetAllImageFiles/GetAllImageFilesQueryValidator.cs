﻿using FluentValidation;
using MongoDB.Bson;

namespace PictureLibrary.Application.Query.GetAllImageFiles;

public class GetAllImageFilesQueryValidator : AbstractValidator<GetAllImageFilesQuery>
{
    public GetAllImageFilesQueryValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().Must((userId) => ObjectId.TryParse(userId, out _));
        RuleFor(x => x.LibraryId).NotEmpty().Must((libraryId) => ObjectId.TryParse(libraryId, out _));
    }
}