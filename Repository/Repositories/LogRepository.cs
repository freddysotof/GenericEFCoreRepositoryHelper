using Repository.Context;
using Repository.Interfaces;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenericRepository.Helpers;
namespace DataService.Repositories
{
    public class LogRepository : StoredProcedureHandler<Log>, ILogRepository
    {
        public LogRepository(RepositoryDbContext context)
        : base(context)
        {
        }
        #region Log Repository

        public async Task<IEnumerable<Log>> GetAllAsync()
            => await base.ExecuteFromSqlRaw(new Log(), ProcedureType.Read);
        public async Task<IEnumerable<Log>> GetAllAsync(Log entity)
        => await base.ExecuteFromSqlRaw(entity, ProcedureType.Read);
        public async Task<IEnumerable<Log>> GetLogsManualProcedureAsync()
        => await base.ExecuteFromSqlRaw("dbo.SP_GetLogs");
        public async Task<Log> GetOneAsync(Log entity)
        {
            var result = await base.ExecuteFromSqlRaw(entity, ProcedureType.Read);
            return result?.SingleOrDefault();
        }
        public async Task<Log> InsertAsync(Log entity)
        {
            var results = await base.ExecuteSqlAsync(entity, ProcedureType.Create);
            entity.LogId = (int)results?.Where(x => x.ParameterName == "@LogId").FirstOrDefault().Value;
            entity.App = results?.Where(x => x.ParameterName == "@App").FirstOrDefault().Value.ToString();
            return entity;
        }
        public async Task<Log> InsertLogManualProcedureAsync(Log log)
        {
            var sqlParameters = new List<SqlParameter> {
                 new SqlParameter {
                     ParameterName = "@LogId",
                    SqlDbType = SqlDbType.Int,
                    Value = DBNull.Value,
                    Direction = ParameterDirection.Output,
                },
                 new SqlParameter {
                     ParameterName = "@Date",
                    SqlDbType = SqlDbType.DateTime,
                    Value = DateTime.Now,
                    Direction = ParameterDirection.Input,
                },
                new SqlParameter {
                     ParameterName = "@Thread",
                    SqlDbType = SqlDbType.VarChar,
                    Value = DBNull.Value,
                    Direction = ParameterDirection.Input,
                },
                  new SqlParameter {
                     ParameterName = "@Level",
                    SqlDbType = SqlDbType.VarChar,
                    Value = DBNull.Value,
                    Direction = ParameterDirection.Input,
                },
                    new SqlParameter {
                     ParameterName = "@Url",
                    SqlDbType = SqlDbType.VarChar,
                    Value = DBNull.Value,
                    Direction = ParameterDirection.Input,
                },
                      new SqlParameter {
                     ParameterName = "@Logger",
                    SqlDbType = SqlDbType.VarChar,
                    Value = DBNull.Value,
                    Direction = ParameterDirection.Input,
                },
                        new SqlParameter {
                     ParameterName = "@Action",
                    SqlDbType = SqlDbType.VarChar,
                    Value = DBNull.Value,
                    Direction = ParameterDirection.Input,
                },
                          new SqlParameter {
                     ParameterName = "@SourceAction",
                    SqlDbType = SqlDbType.VarChar,
                    Value = DBNull.Value,
                    Direction = ParameterDirection.Input,
                },
                      new SqlParameter {
                     ParameterName = "@Message",
                    SqlDbType = SqlDbType.VarChar,
                    Value = DBNull.Value,
                    Direction = ParameterDirection.Input,
                      },
                      new SqlParameter {
                     ParameterName = "@Exception",
                    SqlDbType = SqlDbType.VarChar,
                    Value = DBNull.Value,
                    Direction = ParameterDirection.Input,
                },
                              new SqlParameter {
                     ParameterName = "@App",
                    SqlDbType = SqlDbType.VarChar,
                    Value = DBNull.Value,
                    Direction = ParameterDirection.Output,
                    Size=100
                },
                                      new SqlParameter {
                     ParameterName = "@User",
                    SqlDbType = SqlDbType.VarChar,
                    Value = DBNull.Value,
                    Direction = ParameterDirection.Input,
                },
                                            new SqlParameter {
                     ParameterName = "@RequestBody",
                    SqlDbType = SqlDbType.VarChar,
                    Value = DBNull.Value,
                    Direction = ParameterDirection.Input,
                },
                                                  new SqlParameter {
                     ParameterName = "@Reference",
                    SqlDbType = SqlDbType.VarChar,
                    Value = DBNull.Value,
                    Direction = ParameterDirection.Input,
                },
                                                        new SqlParameter {
                     ParameterName = "@Json",
                    SqlDbType = SqlDbType.VarChar,
                    Value = DBNull.Value,
                    Direction = ParameterDirection.Input,
                }
            };
            var results = await base.ExecuteSqlAsync("dbo.SP_InsertLog", sqlParameters);
            log.LogId = (int)results?.Where(x => x.ParameterName == "@LogId").FirstOrDefault().Value;
            log.App = results?.Where(x => x.ParameterName == "@App").FirstOrDefault().Value.ToString();
            return log;
        }
        public async Task<Log> UpdateAsync(Log entity)
        {
            await base.ExecuteSqlAsync(entity, ProcedureType.Update);
            return entity;
        }
        public async Task<bool> ChangeStatusAsync(Log entity)
        {
            await base.ExecuteSqlAsync(entity, ProcedureType.ChangeStatus);
            return true;
        }
        public async Task<bool> DeleteAsync(int id, string userId)
        {
            var Log = new Log { LogId = id };
            await base.ExecuteSqlAsync(Log, ProcedureType.Delete);
            return true;
        }
        #endregion
    }
}
