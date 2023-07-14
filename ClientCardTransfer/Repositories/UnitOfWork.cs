using ClientCardTransfer.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace ClientCardTransfer.Repositories
{/// <summary>
/// Реализация Unit of Work
/// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private ICardRepository _cardRepository;
        private IClientRepository _clientRepository;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public ICardRepository CardRepository
        {
            get
            {
                if (_cardRepository == null)
                {
                    _cardRepository = new CardRepository(_context);
                }
                return _cardRepository;
            }
        }

        public IClientRepository ClientRepository
        {
            get
            {
                if (_clientRepository == null)
                {
                    _clientRepository = new ClientRepository(_context);
                }
                return _clientRepository;
            }
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
