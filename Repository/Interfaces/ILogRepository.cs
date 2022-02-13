using GenericRepository.Interfaces;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface ILogRepository : IGenericRepository<Log>
    {
        Task<IEnumerable<Log>> GetLogsManualProcedureAsync();
        Task<Log> InsertLogManualProcedureAsync(Log entity);
    }
}
