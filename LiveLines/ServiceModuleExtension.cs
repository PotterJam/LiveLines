using LiveLines.Api.Database;
using LiveLines.Api.Lines;
using LiveLines.Api.Songs;
using LiveLines.Api.Streaks;
using LiveLines.Api.Users;
using LiveLines.Database;
using LiveLines.Lines;
using LiveLines.Songs;
using LiveLines.Streaks;
using LiveLines.Users;

namespace LiveLines;

public static class ServiceModuleExtension
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        // TODO: I should be able to make all of the concrete implementations internal

        return services
            .AddSingleton<IDatabaseCommandExecutor, PostgresCommandExecutor>()
            .AddSingleton<IUserService, UserService>()
            .AddSingleton<IUserStore, UserStore>()
            .AddSingleton<ILinesService, LinesService>()
            .AddSingleton<ILinesStore, LinesStore>()
            .AddSingleton<ISongStore, SongStore>()
            .AddSingleton<ISongService, SongService>()
            .AddSingleton<IStreakService, StreakService>()
            .AddSingleton<IProfileStore, ProfileStore>()
            .AddSingleton<IProfileService, ProfileService>()
            ;
    }
}