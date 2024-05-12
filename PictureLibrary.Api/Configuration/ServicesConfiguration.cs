using FluentValidation;
using MongoDB.Driver;
using PictureLibrary.Application.Configuration;
using PictureLibrary.Application.Mapper;
using PictureLibrary.Domain.Configuration;
using PictureLibrary.Domain.Repositories;
using PictureLibrary.Domain.Services;
using PictureLibrary.Infrastructure.Repositories;
using PictureLibrary.Infrastructure.Services;

namespace PictureLibrary.Api.Configuration
{
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
                .AddTransient<IFileMetadataRepository, FileMetadataRepository>();
        }

        private static IServiceCollection RegisterServicesPrivate(this IServiceCollection services, IConfiguration configuration)
        {
            return services
                .AddSingleton<IAppSettings, AppSettings>()
                .AddMediatR(new MediatRServiceConfiguration().RegisterServicesFromAssembly(typeof(ApplicationEntrypoint).Assembly))
                .AddValidatorsFromAssembly(typeof(ApplicationEntrypoint).Assembly)
                .AddSingleton<IMapper, MapperlyMapper>()
                .AddTransient<IMongoClient, MongoClient>((provider) => new MongoClient(configuration["DbConnectionString"]))
                .AddSingleton<IHashAndSaltService, HashAndSaltService>()
                .AddSingleton<IAuthorizationDataService, AuthorizationDataService>()
                .AddTransient<IMissingRangesParser, MissingRangesParser>()
                .AddTransient<IByteRangesService, ByteRangesService>()
                .AddTransient<IMissingRangesService, MissingRangesService>();
        }
    }
}
