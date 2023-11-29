using Catalog.Service.Data;
using Catalog.Service.Models;
using Catalog.Service.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Catalog.Service
{
    public static class Extensions
    {
        public static IServiceCollection AddSqlite<T>(this IServiceCollection services) where T : DbContext
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

            return services;
        }



        public static IServiceCollection AddRepository<T>(this IServiceCollection services) where T : class, IEntity
        {
            services.AddTransient<IRepository<T>, SqlRepository<T>>();
            return services;
        }

        //public static void RegisterAllEntities<IEntity>(this ModelBuilder modelBuilder, params Assembly[] assemblies)
        //{
        //    IEnumerable<Type> types = assemblies.SelectMany(a => a.GetExportedTypes())
        //    .Where(c => c.IsClass && !c.IsAbstract && c.IsPublic);

        //    foreach (Type type in types)
        //    {
        //        if (type.GetInterfaces().Contains(typeof(IEntity)))
        //            modelBuilder.Entity(type);
        //    }
        //}

    }
}
