using GenericRepository.Repositories;
using Repository.Context;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public class EntityLogRepository : EntityRepository<EntityLog, RepositoryDbContext>
    {
        public EntityLogRepository(RepositoryDbContext context) : base(context)
        {
        }
    }
}
