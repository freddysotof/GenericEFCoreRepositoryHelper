using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericRepository.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> GetOneAsync(T entity);
        Task<IEnumerable<T>> GetAllAsync(); 
        Task<IEnumerable<T>> GetAllAsync(T entity);
        Task<T> InsertAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task<bool> DeleteAsync(int id,string userId);
        Task<bool> ChangeStatusAsync(T entity);

    }
}
