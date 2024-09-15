﻿using PictureLibrary.Client.Model;

namespace PictureLibrary.Client.Clients
{
    public class UsersClient(IApiHttpClient client) : IUsersClient
    {
        public async Task<AuthorizationData> Login(LoginUserRequest request)
        {
            return await client.Post<AuthorizationData>("auth/login", request);
        }

        public async Task<User> Register(AddUserRequest request)
        {
            return await client.Post<User>("user/register", request);
        }

        public async Task<User> GetUser(AuthorizationData authorizationData)
        {
            return await client.Get<User>($"user/get", authorizationData);
        }

        public async Task<User> UpdateUser(UpdateUserRequest request, AuthorizationData authorizationData)
        {
            return await client.Post<User>("user/update", request, authorizationData);
        }
    }
}
