﻿using PictureLibrary.Client.Model;

namespace PictureLibrary.Client
{
    public interface IApiHttpClient
    {
        Task<T> Get<T>(string url, AuthorizationData? authorizationData = null) where T : class;
        Task<T> Post<T>(string url, object data, AuthorizationData? authorizationData = null) where T : class;
    }
}