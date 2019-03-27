using System.Collections.Generic;
using System.Threading.Tasks;
using Reads.Models;

namespace Reads
{
    public interface IEfRepository<T> where T : Entity
    {
        void Add(T entity);
        void Delete(T entity);
        Task<T> Get(int id);
        Task<List<T>> GetAll();
        void Update(T entity);
    }
}