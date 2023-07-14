using System.Threading.Tasks;
using System;

namespace ClientCardTransfer.Repositories
{/// <summary>
/// Интерфейс Unit of Work
/// </summary>
    public interface IUnitOfWork
    {
        ICardRepository CardRepository { get; }
        IClientRepository ClientRepository { get; }
        Task<int> SaveChangesAsync();
    }
}
