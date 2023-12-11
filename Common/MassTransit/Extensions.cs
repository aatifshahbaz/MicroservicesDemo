using Common.Setting;
using MassTransit;
using MassTransit.Transports.Fabric;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;

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

                    var uri = new Uri($"amqps://{rabbitMQSettings.UserName}:{rabbitMQSettings.Password}@{rabbitMQSettings.Host}/{rabbitMQSettings.VHost}");

                    configurator.Host(uri);

                    //configurator.Host(rabbitMQSettings.Host, rabbitMQSettings.VHost, host =>
                    //{
                    //    host.Username(rabbitMQSettings.UserName);
                    //    host.Password(rabbitMQSettings.Password);
                    //    host.UseSsl(s =>
                    //    {
                    //        s.Protocol = SslProtocols.Tls12;
                    //    });
                    //});

                    configurator.ConfigureEndpoints(context, new KebabCaseEndpointNameFormatter(serviceSettings.ServiceName, false));
                });
            });

            services.AddMassTransitHostedService();

            return services;
        }
    }
}
