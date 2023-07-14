using ClientCardTransfer.Data;
using ClientCardTransfer.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientCardTransfer.Repositories
{/// <summary>
/// Реализация репозитория Client
/// </summary>
    public class ClientRepository : Repository<Client>, IClientRepository
    {
        public ClientRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Client>> GetAllActiveClients()
        {
            return await _dbSet.Where(c => c.DeleteDate == null).ToListAsync();
        }
    }
}
