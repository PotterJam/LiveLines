using LiveLines.Api.Database;
using LiveLines.Api.Lines;
using LiveLines.Api.Users;
using LiveLines.Database;
using LiveLines.Lines;
using LiveLines.Security;
using LiveLines.Users;
using Microsoft.Extensions.DependencyInjection;

namespace LiveLines
{
    public static class ServiceModuleExtension
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            return services
                .AddSingleton<IDatabaseCommandExecutor, PostgresCommandExecutor>()
                .AddSingleton<IUserService, UserService>()
                .AddSingleton<IUserStore, UserStore>()
                .AddSingleton<ILinesService, LinesService>()
                .AddSingleton<ILinesStore, LinesStore>();
        }
    }
}