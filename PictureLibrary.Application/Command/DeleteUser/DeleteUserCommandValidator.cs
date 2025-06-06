﻿using FluentValidation;
using MongoDB.Bson;

namespace PictureLibrary.Application.Command;

public class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
{
    public DeleteUserCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().Must(x => ObjectId.TryParse(x, out _));
    }
}