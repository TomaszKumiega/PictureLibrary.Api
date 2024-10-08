﻿using PictureLibrary.Client.BaseClient;
using PictureLibrary.Client.Model;
using PictureLibrary.Client.Requests;

namespace PictureLibrary.Client.Clients.Libraries
{
    internal class LibrariesClient(IApiHttpClient apiHttpClient) : ILibrariesClient
    {
        public async Task<Library> GetLibrary(string id, AuthorizationData authorizationData)
        {
            return await apiHttpClient.Get<Library>($"library/get?id={id}", authorizationData);
        }

        public async Task<AllLibraries> GetAllLibraries(AuthorizationData authorizationData)
        {
            return await apiHttpClient.Get<AllLibraries>("library/getall", authorizationData);
        }

        public async Task<Library> AddLibrary(AddLibraryRequest request, AuthorizationData authorizationData)
        {
            return await apiHttpClient.Post<Library>("library/create", request, authorizationData);
        }

        public async Task<Library> UpdateLibrary(string libraryId, UpdateLibraryRequest request, AuthorizationData authorizationData)
        {
            return await apiHttpClient.Patch<Library>($"library/update?libraryId={libraryId}", request, authorizationData);
        }

        public async Task DeleteLibrary(string id, AuthorizationData authorizationData)
        {
            await apiHttpClient.Delete($"library/delete?id={id}", authorizationData);
        }
    }
}