using FluentValidation;
using PictureLibrary.Application.Configuration;
using PictureLibrary.Application.Mapper;
using PictureLibrary.Domain.Repositories;

namespace PictureLibrary.Api.Configuration
{
    public static class ServicesConfiguration
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            return services
                .RegisterServicesPrivate()
                .RegisterRepositories();
        }

        private static IServiceCollection RegisterRepositories(this IServiceCollection services)
        {
            return services.AddTransient<ILibraryRepository, ILibraryRepository>();
        }

        private static IServiceCollection RegisterServicesPrivate(this IServiceCollection services)
        {
            return services
                .AddMediatR(new MediatRServiceConfiguration())
                .AddValidatorsFromAssembly(typeof(ApplicationEntrypoint).Assembly)
                .AddSingleton<IMapper, MapperlyMapper>();
        }
    }
}
