﻿using MediatR;
using MongoDB.Bson;
using PictureLibrary.Application.Mapper;
using PictureLibrary.Contracts;
using PictureLibrary.Domain.Entities;
using PictureLibrary.Domain.Exceptions;
using PictureLibrary.Domain.Repositories;

namespace PictureLibrary.Application.Command.UpdateUser;

public class UpdateUserHandler(
    IMapper mapper,
    IUserRepository userRepository) : IRequestHandler<UpdateUserCommand, UserDto>
{
    private readonly IMapper _mapper = mapper;
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<UserDto> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        ObjectId userId = ObjectId.Parse(request.UserId);

        User user = _userRepository.FindById(userId) ?? throw new NotFoundException();

        user = UpdateUser(user, request.UserDto);

        await _userRepository.Update(user);

        return _mapper.MapToDto(user);
    }

    private static User UpdateUser(User user, UpdateUserDto userDto)
    {
        user.Email = userDto.Email;
        user.Username = userDto.Username;

        return user;
    }
}