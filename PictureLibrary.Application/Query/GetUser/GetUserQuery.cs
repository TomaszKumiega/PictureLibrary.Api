using MediatR;
using PictureLibrary.Contracts;

namespace PictureLibrary.Application.Query;

public record GetUserQuery(string UserId) : IRequest<UserDto>;