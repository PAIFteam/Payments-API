using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Payments.Core.Application.UseCases.Payment.Processed;
using Payments.Core.Domain.Interfaces;
using Payments.Infra.RabbitMq.Publishers;

namespace Payments.Infra.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfraestructure(this IServiceCollection services)
        {
            //Registro do MediaR
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblies(
                    Assembly.GetExecutingAssembly(),
                    Assembly.GetAssembly(typeof(ProcessedUseCase))!
                    );
            });

            //Registro dos Repositorios

            //Registro dos UseCases
            services.AddScoped<ProcessedUseCase>();

            return services;
        }
    }
}
