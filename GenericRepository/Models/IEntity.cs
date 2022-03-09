using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericRepository.Entities
{
    public interface IEntity
    {
        int? Id { get; set; }
        string CreatedBy { get; set; }
        DateTime CreatedDate { get; set; }
        string UpdatedBy { get; set; }
        DateTime? UpdatedDate { get; set; }
        int? StatusId { get; set; }
    }
}
