using ClientCardTransfer.Data;
using ClientCardTransfer.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientCardTransfer.Repositories
{/// <summary>
/// Реализация репозитория Card
/// </summary>
    public class CardRepository : Repository<Card>, ICardRepository
    {
        public CardRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Card>> GetCardsByClientId(int clientId)
        {
            return await _dbSet.Where(c => c.ClientId == clientId).ToListAsync();
        }
    }
}
