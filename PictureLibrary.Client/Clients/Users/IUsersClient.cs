﻿using PictureLibrary.Client.Model;
using PictureLibrary.Client.Requests;

namespace PictureLibrary.Client.Clients.Users
{
    public interface IUsersClient
    {
        Task<AuthorizationData> Login(LoginUserRequest request);
        Task<User> Register(AddUserRequest request);
        Task<User> GetUser(AuthorizationData authorizationData);
        Task<User> UpdateUser(UpdateUserRequest request, AuthorizationData authorizationData);
        Task DeleteUser(AuthorizationData authorizationData);
    }
}