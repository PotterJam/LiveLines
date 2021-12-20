using LiveLines.Database;
using Microsoft.Extensions.DependencyInjection;

namespace LiveLines
{
    public static class ServiceModuleExtension
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            return services
                .AddSingleton<IDatabaseCommandExecutor, PostgresCommandExecutor>();
        }
    }
}