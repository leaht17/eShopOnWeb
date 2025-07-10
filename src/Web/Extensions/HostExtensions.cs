using System; // ...existing code...
using Microsoft.Extensions.DependencyInjection; // ...existing code...
using Microsoft.Extensions.Logging; // ...existing code...
using Microsoft.AspNetCore.Identity; // ...existing code...
using Microsoft.EntityFrameworkCore; // ...existing code...
using System.Threading.Tasks;
using Microsoft.eShopWeb.Infrastructure.Data;
using Microsoft.eShopWeb.Infrastructure.Identity;

namespace Microsoft.eShopWeb.Web.Extensions;

public static class HostExtensions
{
    public static async Task SeedDatabaseAsync(this IHost host)
    {
        using var scope = host.Services.CreateScope();
        var scopedProvider = scope.ServiceProvider;
        var logger = scopedProvider.GetRequiredService<ILoggerFactory>().CreateLogger("SeedDatabase");
        try
        {
            var catalogContext = scopedProvider.GetRequiredService<CatalogContext>();
            await CatalogContextSeed.SeedAsync(catalogContext, logger);

            var userManager = scopedProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = scopedProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var identityContext = scopedProvider.GetRequiredService<AppIdentityDbContext>();
            await AppIdentityDbContextSeed.SeedAsync(identityContext, userManager, roleManager);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred seeding the DB.");
        }
    }
}
