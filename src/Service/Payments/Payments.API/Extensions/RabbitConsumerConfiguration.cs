
using GreenPipes;
using MassTransit;
using MassTransit.MultiBus;
using Microsoft.Extensions.DependencyInjection;
using Payments.Core.Domain.Entities.RabbitMQ;
using Payments.Infra.RabbitMq.Publishers;
using Payments.Core.Domain.Interfaces;
using Payments.Core.Entities.RabbitMq;

namespace Payments.API.Extensions
{
    public static class RabbitConsumerConfiguration
    {
        public static IServiceCollection AddConsumer(this IServiceCollection services, IConfiguration configuration)
        {
            var rabbitSettings = new RabbitMqConfigurationSettings();
            configuration
                .GetSection(RabbitMqConfigurationSettings.OPTION_KEY)
                .Bind(rabbitSettings);
            if (rabbitSettings.StartConsumer)
            {
                CreateBus<IBus>(services, rabbitSettings);

                services.AddMassTransitHostedService();
            }

            return services;
        }

        private static void CreateBus<T>(IServiceCollection services, RabbitMqConfigurationSettings rabbitSettings) where T : class, IBus
        {
            services.AddMassTransit<IBus>(_ =>
            {
                _.AddConsumer<OrderPlacedEventConsumer>();
                

                _.UsingRabbitMq((context, configure) =>
                {
                    var rabbitUri = new Uri($"rabbitmq://{rabbitSettings.Username}:{rabbitSettings.Password}@{rabbitSettings.HostName}:5672"); ///{rabbitSettings.QueueName}

                    configure.Host(rabbitUri, h =>
                    {
                    });


                    configure.ReceiveEndpoint(rabbitSettings.QueueNameConsumer, e =>
                    {

                        e.ConfigureConsumer<OrderPlacedEventConsumer>(context);
                    });
                    
                });
            });
            
        }
       
    }
}
