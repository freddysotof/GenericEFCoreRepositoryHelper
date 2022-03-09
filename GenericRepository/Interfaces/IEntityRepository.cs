using GenericRepository.Entities;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GenericRepository.Interfaces
{
   
    public interface IEntityRepository<T> where T : class, IEntity
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetOneAsync(int? id);
        Task<T> AddAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task<bool> ChangeStatusAsync(T entity);
        Task<bool> DeleteAsync(int? id);
        Task<List<T>> ExecuteFromSqlRaw(string procedureName, List<SqlParameter> sqlParameters = null);
        Task<IEnumerable<SqlParameter>> ExecuteSqlAsync(string procedureName = null, List<SqlParameter> sqlParameters = null);
    }
}
