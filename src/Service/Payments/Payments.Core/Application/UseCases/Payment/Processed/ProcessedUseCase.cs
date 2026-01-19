using Microsoft.Extensions.Logging;
using Payments.Core.Domain.Entities.Base;
using Payments.Core.Domain.Entities.RabbitMQ;
using Payments.Core.Domain.Interfaces;


namespace Payments.Core.Application.UseCases.Payment.Processed
{
    public class ProcessedUseCase
    {
        private readonly RabbitMqConfigurationSettings _rabbitMqConfigurationSettings;
        private readonly IPublisher _publisher;
        private readonly ILogger<ProcessedUseCase> _logger;
       

        public ProcessedUseCase(
                RabbitMqConfigurationSettings rabbitMqConfigurationSettings,
                IPublisher publisher,
                ILogger<ProcessedUseCase> logger
        )
        {
            _rabbitMqConfigurationSettings = rabbitMqConfigurationSettings;
            _publisher = publisher;
            _logger = logger;
        }

        public async Task<ProcessedOutPut> ExecuteAsync(ProcessedInput input)
        {

            _logger.LogInformation("Starting ProcessedUseCase.ExecuteAsync -USE CASE DE PROCESMENTO DE PAGAMENTO");
            _logger.LogInformation("......");
            _logger.LogInformation("......");

            var paymentAproved = false;
            var messageRespText = "";

            try
            {
                
                OutPutBase outPutBase = ValidateInput(input);

                if (!outPutBase.Result)
                {
                    return new ProcessedOutPut
                    {
                        Result = false,
                        Message = outPutBase.Message,
                        Exception = outPutBase.Exception
                    };
                }

                // simular a lógica de envio do Pagamento
                // Caso não haja saldo, lançar uma exceção para simular falha no pagamento
                if (input.Price > 380)
                {
                    //Rejeitado por falta de Saldo
                    paymentAproved = false;
                    messageRespText = "Pagamento Rejeitado por falta de saldo";
                    _logger.LogInformation("Pagamento RECUSADO!!! por falta de saldo");
                }
                else
                {
                    //Pagamento Aprovado
                    paymentAproved = true;
                    messageRespText = "Aprovado com sucesso";
                    _logger.LogInformation("Pagamento APROVADO");
                }

                var messageResp = new PaymentProcessedMessage(input.IdUser,
                                                              input.IdGame,
                                                              input.Price,
                                                              paymentAproved,
                                                              messageRespText);
                // Publicar a mensagem na fila RabbitMQ Evento: PaymentProcessedEvent
                _logger.LogInformation(".....");
                _logger.LogInformation(".....");
                _logger.LogInformation("Publicando mensagem de PaymentProcessedMessage na fila RabbitMQ - Resposta do Processamento");
                await _publisher.Publish(messageResp, _rabbitMqConfigurationSettings.GetQueueAdress());
                await _publisher.Publish(messageResp, _rabbitMqConfigurationSettings.GetQueueAdressMessage());
                ProcessedOutPut outPut = new ProcessedOutPut
                {                    
                    Result = true,
                    Message = "Payment executed successfully",
                    Exception = null
                };

                return outPut;
            }
            catch (Exception ex)
            {
                return new ProcessedOutPut
                {
                    Result = false,
                    Message = "Ocorreu umm erro de Runtime Interno",
                    Exception = ex
                };
                
            }

        }

        private OutPutBase ValidateInput(ProcessedInput input)
        {
            
            OutPutBase outPut = new OutPutBase();
            
            outPut.Message = "E-mail já cadastrado";
            outPut.Result = true;

            // Implement validation logic here
            return outPut;
        }
        
    }
}
