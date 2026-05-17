using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Payments.Core.Application.UseCases.Payment.Processed;
using Payments.Core.Domain.Entities.RabbitMQ;
using Payments.Core.Domain.Interfaces;

namespace Payments.UnitTests;

public class ProcessedUseCaseTests
{
    [Theory]
    [InlineData(100, true, "Aprovado com sucesso")]
    [InlineData(381, false, "Pagamento Rejeitado por falta de saldo")]
    public async Task ExecuteAsync_PrecoVariavel_DevePublicarResultadoEsperado(decimal price, bool expectedApproval, string expectedMessage)
    {
        var publisher = new Mock<IPublisher>();
        var publishedMessages = new List<PaymentProcessedMessage>();
        publisher.Setup(x => x.Publish(It.IsAny<PaymentProcessedMessage>(), It.IsAny<Uri>()))
            .Callback<object, Uri>((msg, _) => publishedMessages.Add((PaymentProcessedMessage)msg))
            .Returns(Task.CompletedTask);

        var settings = new RabbitMqConfigurationSettings
        {
            HostName = "localhost",
            Username = "guest",
            Password = "guest",
            QueueName = "payment_processed",
            QueueNameMessage = "payment_message",
            QueueNameConsumer = "order_placed",
            RedeliveryInSeconds = [],
            RetryInSeconds = []
        };

        var sut = new ProcessedUseCase(settings, publisher.Object, Mock.Of<ILogger<ProcessedUseCase>>());

        var result = await sut.ExecuteAsync(new ProcessedInput(1, 2, price));

        result.Result.Should().BeTrue();
        publishedMessages.Should().HaveCount(2);
        publishedMessages.Should().OnlyContain(x => x.Aproved == expectedApproval && x.Message == expectedMessage);
    }
}
