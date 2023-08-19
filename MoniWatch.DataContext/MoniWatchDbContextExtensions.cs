using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace MoniWatch.DataContext;

public static class MoniWatchDbContextExtensions
{
    public static IServiceCollection AddMoniWatchContext(
        this IServiceCollection services, string relativePath=".."
    )
    {
        string databasePath = Path.Combine(relativePath, "AppBase.db");
        services.AddDbContext<MoniWatchDbContext>(options => {
            options.UseSqlite($"Data Source={databasePath}");
            options.LogTo(Console.WriteLine,
            new[] {Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.CommandExecuting});
        });
        return services;
    }
}