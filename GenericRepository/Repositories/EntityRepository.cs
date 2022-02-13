using GenericRepository.Entities;
using GenericRepository.Extensions;
using GenericRepository.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace GenericRepository.Repositories
{
    public abstract class EntityRepository<TEntity, TContext> : IEntityRepository<TEntity>
          where TEntity : class, IEntity
          where TContext : DbContext
    {
        private readonly TContext _context;
        private readonly DbSet<TEntity> _collections;
        public EntityRepository(TContext context)
        {
            this._context = context;
            this._collections = context.Set<TEntity>();
        }
        public virtual async Task<TEntity> AddAsync(TEntity entity)
        {
            _collections.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public virtual async Task<TEntity> DeleteAsync(int id)
        {
            var entity = await _collections.FindAsync(id);
            if (entity == null)
            {
                return entity;
            }

            _collections.Remove(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public virtual async Task<TEntity> GetOneAsync(int id)
        {
            return await _collections.FindAsync(id);
        }

        public virtual Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return Task.FromResult(_collections.AsEnumerable());
        }

        public virtual async Task<TEntity> UpdateAsync(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return entity;
        }
        public async Task<IEnumerable<SqlParameter>> ExecuteSqlAsync(string procedureName = null, List<SqlParameter> sqlParameters = null)
        {
            string sqlRaw = $"{procedureName} {sqlParameters.JoinSqlParameters()}";
            //int response;
            if (sqlParameters == null)
                await _context.Database.ExecuteSqlRawAsync(sqlRaw);
            else
                await _context.Database.ExecuteSqlRawAsync(sqlRaw, sqlParameters);
            return sqlParameters.Where(x => x.Direction == ParameterDirection.Output);
        }
        public virtual Task<List<TEntity>> ExecuteFromSqlRaw(string procedureName, List<SqlParameter> sqlParameters = null)
        {
            List<TEntity> result;
            if (sqlParameters == null)
                result = _collections.FromSqlRaw($"exec {procedureName}").ToList();
            else
                result = _collections.FromSqlRaw($"exec {procedureName} {sqlParameters.JoinSqlParameters()}", sqlParameters?.ToArray())
                  .ToList();

            return Task.FromResult(result);
        }

 
    }

    //public class EFGenericRepository<TEntity> : IEFGenericRepository<TEntity> where TEntity : class, new()
    //{
    //    protected readonly BonaDbContext _context;

    //    public EFGenericRepository(BonaDbContext context)
    //    {
    //        _context = context ?? throw new ArgumentNullException(nameof(context));
    //    }

    //    public IEnumerable<TEntity> GetAll()
    //    {
    //        try
    //        {
    //            return __collections;
    //        }
    //        catch (Exception ex)
    //        {
    //            throw new Exception($"Couldn't retrieve entities: {ex.Message}");
    //        }
    //    }

    //    public async Task<TEntity> AddAsync(TEntity entity)
    //    {
    //        if (entity == null)
    //        {
    //            throw new ArgumentNullException($"{nameof(AddAsync)} entity must not be null");
    //        }

    //        try
    //        {
    //            await _context.AddAsync(entity);
    //            await _context.SaveChangesAsync();

    //            return entity;
    //        }
    //        catch (Exception ex)
    //        {
    //            throw new Exception($"{nameof(entity)} could not be saved: {ex.Message}");
    //        }
    //    }

    //    public async Task<TEntity> UpdateAsync(TEntity entity)
    //    {
    //        if (entity == null)
    //        {
    //            throw new ArgumentNullException($"{nameof(AddAsync)} entity must not be null");
    //        }

    //        try
    //        {
    //            _context.Entry(entity).State = EntityState.Modified;
    //            await _context.SaveChangesAsync();

    //            return entity;
    //        }
    //        catch (Exception ex)
    //        {
    //            throw new Exception($"{nameof(entity)} could not be updated: {ex.Message}");
    //        }
    //    }
    //}
}
