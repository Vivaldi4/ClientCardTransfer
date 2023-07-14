using ClientCardTransfer.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace ClientCardTransfer.Repositories
{/// <summary>
/// Реализация Unit of Work
/// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private Lazy<ICardRepository> _cardRepository;
        private Lazy<IClientRepository> _clientRepository;
        public ICardRepository CardRepository => _cardRepository.Value;
        public IClientRepository ClientRepository => _clientRepository.Value;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            _cardRepository = new Lazy<ICardRepository>(() => new CardRepository(_context));
            _clientRepository = new Lazy<IClientRepository>(() => new ClientRepository(_context));
        }
        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
