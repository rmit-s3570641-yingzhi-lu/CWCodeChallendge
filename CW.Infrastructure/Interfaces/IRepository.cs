using System.Collections.Generic;
using System.Threading.Tasks;
using CW.Infrastructure.Models;

namespace CW.Infrastructure.Interfaces
{
    public interface IRepository<T> where T : BaseEntity
    {
        Task<T> GetByIdAsync(string id);
        Task<List<T>> ListAsync();
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteByIdAsync(string id);
    }
}
