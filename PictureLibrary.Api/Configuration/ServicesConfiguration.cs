using FluentValidation;
using MongoDB.Driver;
using PictureLibrary.Api.ErrorMapping;
using PictureLibrary.Api.ErrorMapping.ExceptionMapper;
using PictureLibrary.Application.Configuration;
using PictureLibrary.Application.Mapper;
using PictureLibrary.Domain.Configuration;
using PictureLibrary.Domain.Repositories;
using PictureLibrary.Domain.Services;
using PictureLibrary.Infrastructure.Repositories;
using PictureLibrary.Infrastructure.Services;

namespace PictureLibrary.Api.Configuration;

public static class ServicesConfiguration
{
    public static IServiceCollection RegisterServices(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .RegisterServicesPrivate(configuration)
            .RegisterRepositories();
    }

    private static IServiceCollection RegisterRepositories(this IServiceCollection services)
    {
        return services
            .AddTransient<ILibraryRepository, LibraryRepository>()
            .AddTransient<IUserRepository, UserRepository>()
            .AddTransient<ITagRepository, TagRepository>()
            .AddTransient<IAuthorizationDataRepository, AuthorizationDataRepository>()
            .AddTransient<IUploadSessionRepository, UploadSessionRepository>()
            .AddTransient<IFileMetadataRepository, FileMetadataRepository>()
            .AddTransient<IImageFileRepository, ImageFileRepository>();
    }

    private static IServiceCollection RegisterServicesPrivate(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddSingleton<IAppSettings, AppSettings>()
            .AddMediatR(new MediatRServiceConfiguration().RegisterServicesFromAssembly(typeof(IApplicationEntrypoint).Assembly))
            .AddValidatorsFromAssembly(typeof(IApplicationEntrypoint).Assembly)
            .AddSingleton<IMapper, MapperlyMapper>()
            .AddTransient<IMongoClient, MongoClient>((_) => new MongoClient(configuration.GetConnectionString("MongoDb")))
            .AddSingleton<IHashAndSaltService, HashAndSaltService>()
            .AddSingleton<IAuthorizationDataService, AuthorizationDataService>()
            .AddTransient<IMissingRangesParser, MissingRangesParser>()
            .AddTransient<IByteRangesService, ByteRangesService>()
            .AddTransient<IMissingRangesService, MissingRangesService>()
            .AddTransient<IFileUploadService, FileUploadService>()
            .AddTransient<IFileService, FileService>()
            .AddTransient<IFileWrapper, FileWrapper>()
            .AddTransient<IFileMetadataUpdateService, FileMetadataUpdateService>()
            .AddTransient<IImageThumbnailCreator, ImageThumbnailCreator>()
            .AddTransient<IPathsProvider, PathsProvider>()
            .AddTransient<IImageFileUpdateService, ImageFileUpdateService>()
            .AddTransient<IExceptionMapper, ExceptionMapper>();
    }
}