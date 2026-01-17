
using GreenPipes;
using MassTransit;
using MassTransit.MultiBus;
using Microsoft.Extensions.DependencyInjection;
using Payments.Core.Domain.Entities.RabbitMQ;
using Payments.Core.Domain.Interfaces;
using Payments.Core.Entities.RabbitMq;
using Payments.Infra.RabbitMq.Publishers;

namespace Payments.API.Extensions
{
    public static class RabbitMqSettings
    {
        public static IServiceCollection AddRabbitMq(this IServiceCollection services, IConfiguration configuration)
        {
            var rabbitSettings = new RabbitMqConfigurationSettings();

            configuration
                .GetSection(RabbitMqConfigurationSettings.OPTION_KEY)
                .Bind(rabbitSettings);
            services.AddScoped<IPublisher, Publisher>();
            services.AddScoped(_ => rabbitSettings);
            services.AddConsumer(configuration);
            

            return services;
        }
       
    }
}
