using ClientCardTransfer.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClientCardTransfer.Repositories
{/// <summary>
/// Интерфейс репозитория Card
/// </summary>
    public interface ICardRepository : IRepository<Card>
    {
        Task<IEnumerable<Card>> GetCardsByClientId(int clientId);
        Task ClearAllCards();
    }
}
