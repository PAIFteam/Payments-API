using Catalog.Core.Domain.Entities.RabbitMQ;
using MassTransit;
using Microsoft.Extensions.Logging;
using Payments.Core.Application.UseCases.Payment.Processed;
using Payments.Core.Domain.Interfaces;
using Quartz.Logging;

namespace Payments.Core.Entities.RabbitMq
{
    public class OrderPlacedEventConsumer: IConsumer<OrderPlacedMessage>
    {
        private readonly ProcessedUseCase _processedUseCase;
        private readonly ILogger<OrderPlacedEventConsumer> _logger;

        public OrderPlacedEventConsumer(
            ProcessedUseCase processedUseCase,
            ILogger<OrderPlacedEventConsumer> logger
            )
        {
            _processedUseCase = processedUseCase;
            _logger = logger;
        }
        public async Task Consume(ConsumeContext<OrderPlacedMessage> context)
        {

            _logger.LogInformation("OrderPlacedEventConsumer - Iniciando consumo da mensagem de OrderPlacedMessage");
            _logger.LogInformation("......");
            _logger.LogInformation("Enviar dados para processadora de Pagamento");
            _logger.LogInformation("......");
            _logger.LogInformation("......");

            await Task.CompletedTask;

            ProcessedOutPut _processedInput = await _processedUseCase.ExecuteAsync(
                new ProcessedInput (context.Message.IdUser,context.Message.IdGame,context.Message.Price));
         

            _logger.LogInformation($"Enviando dados para execução de pagamento,  {context.Message.IdUser} " +
                $" IdGame ({context.Message.IdGame}) e Price {context.Message.Price.ToString()}");
     

            // Simulação de envio de e-mail ou outra ação
            
        }
    }
}
