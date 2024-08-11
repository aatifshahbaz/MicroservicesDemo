using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace Common.Kafka
{
    public static class Extensions
    {
        public static IServiceCollection AddKafkaConsumers(this IServiceCollection services)
        {
            var configuration = services.BuildServiceProvider().GetService<IConfiguration>();

            var assembly = Assembly.GetEntryAssembly();
            // Find all non-abstract classes that implement IConsumer<T>
            var consumerTypes = assembly?.GetTypes().Where(t => !t.IsAbstract && t.GetInterfaces()
                                .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IConsumer<>))).ToList();

            foreach (var consumerType in consumerTypes)
            {
                var consumerInterface = consumerType.GetInterfaces().First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IConsumer<>));

                // Register the Consumer
                services.AddScoped(consumerInterface, consumerType);

                var messageType = consumerInterface.GetGenericArguments()[0];
                var backgroundServiceType = typeof(ConsumerService<>).MakeGenericType(messageType);

                //Registering the background service this way allow multiple instances of same service to get registered
                services.AddSingleton<IHostedService>(provider =>
                {
                    var scope = provider.CreateScope();
                    var consumer = scope.ServiceProvider.GetRequiredService(consumerInterface);
                    var instance = Activator.CreateInstance(backgroundServiceType, configuration, consumer);
                    return instance as BackgroundService;
                });

                //Registering consumer's background services like this will only register the First instance only, and its actually by design
                //services.AddHostedService(provider =>
                //{
                //    var scope = provider.CreateScope();
                //    var consumer = scope.ServiceProvider.GetRequiredService(consumerInterface);
                //    var instance = Activator.CreateInstance(backgroundServiceType, configuration, consumer);
                //    return instance as BackgroundService;
                //});
            }

            return services;
        }
    }
}
