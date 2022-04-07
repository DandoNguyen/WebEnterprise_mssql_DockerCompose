using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using WebEnterprise_mssql.Api.Data;

namespace WebEnterprise_mssql.Api
{
    public static class MigrateDatabase
    {
        public static IHost Migrate<T>(this IHost webHost) where T : ApiDbContext 
        {
            using (var scope = webHost.Services.CreateScope()) {
                var services = scope.ServiceProvider;
                try
                {
                    var db = services.GetService<T>();
                    db.Database.Migrate();
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An Error occurred while migrating the database");
                }
            }
            return webHost;
        }
    }
}