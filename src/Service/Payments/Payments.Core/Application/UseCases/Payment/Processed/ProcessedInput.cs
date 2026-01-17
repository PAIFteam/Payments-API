using Payments.Core.Domain;
using Payments.Core.Domain.Entities;

namespace Payments.Core.Application.UseCases.Payment.Processed
{
    public class ProcessedInput
    {
        public ProcessedInput(int idUser, int idGame, decimal price)
        {
            IdUser = idUser;
            IdGame = idGame;
            Price = price;

        }
        public int IdUser { get; set; }
        public int IdGame { get; set; }
        public decimal Price { get; set; }

    }
}
