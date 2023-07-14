using ClientCardTransfer.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClientCardTransfer.Repositories
{/// <summary>
/// Интерфейс репозитория Client
/// </summary>
    public interface IClientRepository : IRepository<Client>
    {
        Task<IEnumerable<Client>> GetAllActiveClients();
        Task<Client> GetClientByExtenalId(string extenalId);
        Task ClearAllClients();
    }
}
