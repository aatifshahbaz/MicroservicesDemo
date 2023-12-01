using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Common.Data
{
    public static class Extensions
    {
        public static IServiceCollection AddSqlite<T>(this IServiceCollection services) where T : DbContext, IDbContext
        {
            //All below returns same in this context
            //var migrationAssembly = typeof(Program).Assembly.GetName().Name;
            //var migrationAssembly2 = Assembly.GetExecutingAssembly().GetName().Name;

            var migrationAssembly = Assembly.GetEntryAssembly().GetName().Name;

            var configuration = services.BuildServiceProvider().GetService<IConfiguration>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<T>(options =>
            {
                options.UseSqlite(connectionString, sqlOpts => sqlOpts.MigrationsAssembly(migrationAssembly));
            });

            //Injecting already registered CustomDbContext as IDbContext for SqlRepository to Consume it
            services.AddTransient<IDbContext>(provider => provider.GetService<T>());

            return services;
        }

    }
}
