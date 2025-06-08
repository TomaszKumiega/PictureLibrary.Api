using MediatR;
using PictureLibrary.Contracts;

namespace PictureLibrary.Application.Query.GetUser;

public record GetUserQuery(string UserId) : IRequest<UserDto>;