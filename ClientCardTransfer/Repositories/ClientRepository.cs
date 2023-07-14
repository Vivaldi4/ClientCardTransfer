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
        public async Task<Client> GetClientByExtenalId(string extenalId)
        {
            return await _dbSet.FirstOrDefaultAsync(c => c.ExtenalId == extenalId);
        }

        public async Task<IEnumerable<Client>> GetAllActiveClients()
        {
            return await _dbSet.Where(c => c.DeleteDate == null).ToListAsync();
        }
        public async Task ClearAllClients()
        {
            _dbSet.RemoveRange(_dbSet);
            await _context.SaveChangesAsync();
        }
    }
}
