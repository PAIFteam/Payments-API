namespace Payments.API.Payments.CreatePayment; 

public record CreatePaymentRequest(decimal Amount, PaymentMethod PaymentMethod, 
    PaymentStatus PaymentStatus, DateTime CreatedAt, DateTime PaidAt);
public record CreatePaymentResponse(Guid PaymentId);
public class CreatePaymentEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/payments", async (CreatePaymentRequest request, ISender sender) =>
        {
            var command = request.Adapt<CreatePaymentCommand>();
            var result = await sender.Send(command);
            var response = result.Adapt<CreatePaymentResponse>();

            return Results.Created($"/payments/{result.PaymentId}", response);
        })
        .WithName("CreatePayment")
        .Produces<CreatePaymentResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Create Payment")
        .WithDescription("Create Payment");
    }
}
