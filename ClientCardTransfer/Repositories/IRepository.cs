using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClientCardTransfer.Repositories
{/// <summary>
/// Интерфейс репозитория
/// </summary>
/// <typeparam name="T"></typeparam>
    public interface IRepository<T>
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> GetById(int id);
        Task AddRange(IEnumerable<T> entities);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
