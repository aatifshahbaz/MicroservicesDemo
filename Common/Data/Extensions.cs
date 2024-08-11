using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Data
{
    public static class Extensions
    {
        public static IServiceCollection AddSqlite<T>(this IServiceCollection services)
            where T : DbContext, IDbContext
        {
            //All below returns same if used in program.cs file
            //var migrationAssembly = typeof(Program).Assembly.GetName().Name;
            //var migrationAssembly2 = Assembly.GetExecutingAssembly().GetName().Name; //returns common
            //var migrationAssembly = Assembly.GetEntryAssembly().GetName().Name;  //returns calling assembly

            var configuration = services.BuildServiceProvider().GetService<IConfiguration>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            //If we dont provide migration assembly, DbContext of assembly that calls this method will be considered
            //and migration will be generated in the calling assembly(project) as well
            services.AddDbContext<T>(options =>
            {
                options.UseSqlite(connectionString); //, sqlOpts => sqlOpts.MigrationsAssembly(migrationAssembly));
            });

            //Target Assembly == Migration Assembly
            //Code-First Migration aren't working here because bydefault sql migration assembly is "Common" and
            //if we try to add-migration from "inventory.service" assembly, it will throw assembly mismatch error
            //But at runtime we will be able to get target project assembly via GetEntryAssembly()

            //Injecting already registered CustomDbContext as IDbContext for SqlRepository to Consume it
            services.AddTransient<IDbContext>(provider => provider.GetService<T>());

            return services;
        }

    }
}
