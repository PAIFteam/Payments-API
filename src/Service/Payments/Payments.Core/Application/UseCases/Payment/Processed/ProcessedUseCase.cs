using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Logging;
using Payments.Core.Application.UseCases.Payment.Processed;
using Payments.Core.Domain.Entities.Base;
using Payments.Core.Domain.Entities.RabbitMQ;
using Payments.Core.Domain.Interfaces;
using System.Text.RegularExpressions;

namespace Users.Core.Application.UseCases.Users.PutUser
{
    public class ProcessedUseCase
    {
        private readonly RabbitMqConfigurationSettings _rabbitMqConfigurationSettings;
        private readonly ILogger<ProcessedUseCase> _logger;
        private readonly IPublisher _publisher;

        public ProcessedUseCase(RabbitMqConfigurationSettings rabbitMqConfigurationSettings,
               IPublisher publisher,
               ILogger<ProcessedUseCase> logger
        )
        {
            _rabbitMqConfigurationSettings = rabbitMqConfigurationSettings;
            _publisher = publisher;
            _logger = logger;
        }

        public async Task<ProcessedOutput> ExecuteAsync(ProcessedInput input)
        {

            _logger.LogInformation("Starting PutUserUseCase.ExecuteAsync");

            var paymentAproved = false;
            var messageRespText = "";

            try
            {
                
                OutPutBase outPutBase = ValidateInput(input);

                if (!outPutBase.Result)
                {
                    return new ProcessedOutput
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
                }
                else
                {
                    //Pagamento Aprovado
                    paymentAproved = true;
                    messageRespText = "Aprovado com sucesso";

                }

                var messageResp = new PaymentProcessedMessage(input.IdUser,
                                                              input.IdGame,
                                                              input.Price,
                                                              paymentAproved,
                                                              messageRespText);
                // Publicar a mensagem na fila RabbitMQ Evento: PaymentProcessedEvent
                await _publisher.Publish(messageResp, _rabbitMqConfigurationSettings.GetQueueAdress());

                ProcessedOutput outPut = new ProcessedOutput
                {                    
                    Result = true,
                    Message = "Payment executed successfully",
                    Exception = null
                };

                return outPut;
            }
            catch (Exception ex)
            {
                return new ProcessedOutput
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
