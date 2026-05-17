using Catalog.Core.Domain.Entities.RabbitMQ;
using FluentAssertions;
using MassTransit;
using Microsoft.Extensions.Logging;
using Moq;
using Payments.Core.Application.UseCases.Payment.Processed;
using Payments.Core.Domain.Entities.RabbitMQ;
using Payments.Core.Domain.Interfaces;
using Payments.Core.Entities.RabbitMq;

namespace Payments.UnitTests;

public class OrderPlacedEventConsumerTests
{
    [Fact]
    public async Task Consume_MensagemValida_DeveExecutarProcessamentoEPublicarResultado()
    {
        var publisher = new Mock<IPublisher>();
        var published = new List<PaymentProcessedMessage>();
        publisher.Setup(x => x.Publish(It.IsAny<PaymentProcessedMessage>(), It.IsAny<Uri>()))
            .Callback<object, Uri>((msg, _) => published.Add((PaymentProcessedMessage)msg))
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

        var useCase = new ProcessedUseCase(settings, publisher.Object, Mock.Of<ILogger<ProcessedUseCase>>());
        var sut = new OrderPlacedEventConsumer(useCase, Mock.Of<ILogger<OrderPlacedEventConsumer>>());
        var context = new Mock<ConsumeContext<OrderPlacedMessage>>();
        context.SetupGet(x => x.Message).Returns(new OrderPlacedMessage(4, 8, 150m));

        await sut.Consume(context.Object);

        published.Should().HaveCount(2);
        published.Should().OnlyContain(x => x.IdUser == 4 && x.IdGame == 8 && x.Aproved);
    }
}
