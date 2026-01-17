using Catalog.Core.Domain.Entities.RabbitMQ;
using MassTransit;
using Microsoft.Extensions.Logging;
using Payments.Core.Application.UseCases.Payment.Processed;
using Payments.Core.Domain.Interfaces;
using Users.Core.Application.UseCases.Users.PutUser;

namespace Payments.Core.Entities.RabbitMq
{
    public class OrderPlacedEventConsumer: IConsumer<OrderPlacedMessage>
    {
        private readonly IProcessedUseCase _processedUseCase;
        private readonly ILogger<OrderPlacedEventConsumer> _logger;

        public OrderPlacedEventConsumer(
            IProcessedUseCase processedUseCase,
            ILogger<OrderPlacedEventConsumer> logger
            )
        {
            _processedUseCase = processedUseCase;
            _logger = logger;
        }
        public async Task Consume(ConsumeContext<OrderPlacedMessage> context)
        {

            ProcessedOutput _processedInput = await _processedUseCase.ExecuteAsync(
                new ProcessedInput (context.Message.IdUser,context.Message.IdGame,context.Message.Price));


            // Lógica para enviar notificação de boas-vindas ao cliente
            Console.WriteLine($"Enviando dados para execução de pagamento,  {context.Message.IdUser} " +
                $" IdGame ({context.Message.IdGame}) e Price {context.Message.Price.ToString()}");
            // Simulação de envio de e-mail ou outra ação
            await Task.CompletedTask;
        }
    }
}
