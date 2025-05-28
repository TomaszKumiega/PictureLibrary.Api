using PictureLibrary.Client.Authorization;
using PictureLibrary.Client.BaseClient;
using PictureLibrary.Client.Clients.Authorization;
using PictureLibrary.Client.Clients.Libraries;
using PictureLibrary.Client.Clients.Tags;
using PictureLibrary.Client.Clients.Users;
using PictureLibrary.Client.ErrorHandling;
using PictureLibrary.Contracts;

namespace PictureLibrary.Client;

internal class PictureLibraryApiClient : IPictureLibraryApiClient
{
    private readonly ITagsClient _tagsClient;
    private readonly IUsersClient _usersClient;
    private readonly ILibrariesClient _librariesClient;
    private readonly IAuthorizationClient _authorizationClient;

    public PictureLibraryApiClient(HttpClient httpClient, UserAuthorizationDataDto? userAuthorizationDataDto = null)
    {
        AuthorizationDataStore authorizationDataStore = new AuthorizationDataStore()
        {
            UserAuthorizationDataDto = userAuthorizationDataDto
        };
            
        ErrorHandler errorHandler = new ErrorHandler();
        _authorizationClient = new AuthorizationClient(httpClient, errorHandler, authorizationDataStore);
        IApiHttpClient apiHttpClient = new ApiHttpClient(httpClient, errorHandler, _authorizationClient);
            
        _usersClient = new UsersClient(apiHttpClient);
        _tagsClient = new TagsClient(apiHttpClient);
        _librariesClient = new LibrariesClient(apiHttpClient);
    }
        
    public IAuthorizationClient Authorization() => _authorizationClient;
    public IUsersClient Users() => _usersClient;
    public ITagsClient Tags() => _tagsClient;
    public ILibrariesClient Libraries() => _librariesClient;
}