using Common.Entity;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Repository
{
    public static class Extensions
    {
        public static IServiceCollection AddRepository<T>(this IServiceCollection services) where T : class, IEntity
        {
            services.AddTransient<IRepository<T>, SqlRepository<T>>();
            return services;
        }
    }
}
