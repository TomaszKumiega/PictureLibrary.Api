using PictureLibrary.Domain.Repositories;

namespace PictureLibrary.Api.Configuration
{
    public static class ServicesConfiguration
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            return services
                .RegisterRepositories();
        }

        private static IServiceCollection RegisterRepositories(this IServiceCollection services)
        {
            return services.AddTransient<ILibraryRepository, ILibraryRepository>();
        }
    }
}
