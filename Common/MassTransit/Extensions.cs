using Common.Setting;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.Security.Authentication;

namespace Common.MassTransit
{
    public static class Extensions
    {
        public static IServiceCollection AddMassTransitWithRabbitMQ(this IServiceCollection services)
        {
            services.AddMassTransit(configure =>
            {
                configure.AddConsumers(Assembly.GetEntryAssembly());

                configure.UsingRabbitMq((context, configurator) =>
                {
                    var configuration = context.GetService<IConfiguration>();
                    var serviceSettings = configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>(); //Install this for Get<T> Microsoft.Extensions.Configuration.Binder
                    var rabbitMQSettings = configuration.GetSection(nameof(RabbitMQSettings)).Get<RabbitMQSettings>();

                    //Bydefault amqps uses port 5671 and amqp uses port 5672, so we dont have to mention it while using URI for configuring HOST
                    //var uri = new Uri($"amqps://{rabbitMQSettings.UserName}:{rabbitMQSettings.Password}@{rabbitMQSettings.Host}/{rabbitMQSettings.VHost}");
                    //configurator.Host(uri);


                    configurator.Host(rabbitMQSettings.Host, rabbitMQSettings.Port, rabbitMQSettings.VHost, host =>
                    {
                        host.Username(rabbitMQSettings.UserName);
                        host.Password(rabbitMQSettings.Password);

                        if (rabbitMQSettings.Port == 5671)
                        {
                            host.UseSsl(s =>
                            {
                                s.Protocol = SslProtocols.Tls12;
                            });
                        }
                    });

                    //Retry is useful when exception occured due to temporal failures and retrying might eventually solve the problem
                    //Following and more middlewares kicked-in whenever any exception is encounterd in classes that implements IConsumer
                    //Read for more details https://masstransit.io/documentation/concepts/exceptions

                    configurator.UseMessageRetry(retryConf =>
                    {
                        retryConf.Interval(3, TimeSpan.FromSeconds(5));
                    });

                    configurator.ConfigureEndpoints(context, new KebabCaseEndpointNameFormatter(serviceSettings.ServiceName, false));
                });
            });

            //Deprecated after MassTransit v8, no need to do it manually, its will register Bus automatically
            //services.AddMassTransitHostedService();

            return services;
        }


        public static IServiceCollection AddMassTransitWithAzureServiceBus(this IServiceCollection services)
        {
            services.AddMassTransit(configure =>
            {
                configure.AddConsumers(Assembly.GetEntryAssembly());

                configure.UsingAzureServiceBus((context, configurator) =>
                {
                    var configuration = context.GetService<IConfiguration>();
                    var azureServiceBus = configuration.GetConnectionString("AzureServiceBus");
                    var serviceSettings = configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>(); //Install this for Get<T> Microsoft.Extensions.Configuration.Binder

                    configurator.Host(azureServiceBus);

                    configurator.UseMessageRetry(retryConf =>
                    {
                        retryConf.Interval(3, TimeSpan.FromSeconds(5));
                    });

                    //It will auto create the relevent Topics, Subscriptions and Queues, no need to do it manually, it will complicate things
                    //Whenever any message is published at topic, it will be forwarded to respective subscrption for subscriber to consume it.
                    //You wont be able to view any message in Topic, it just forward the messages to all active subscribers
                    //If auto-forward queue is enabled in Subcription, you will be able to view that message via Service Bus Explorer on that Queue
                    //Once that message is consumed by any Subscriber, it will be deleted from Queue as well.
                    configurator.ConfigureEndpoints(context, new KebabCaseEndpointNameFormatter(serviceSettings.ServiceName, false));
                });
            });

            //Deprecated after MassTransit v8, no need to do it manually, its will register Bus automatically
            //services.AddMassTransitHostedService();

            return services;
        }
    }
}
