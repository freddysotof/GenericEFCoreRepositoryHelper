using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericRepository.Models
{
    //public interface IEntity
    //{
    //    int? Id { get; set; }
    //    ActionType ActionType { get; set; }
    //    bool Editable { get => Id.HasValue; }
    //}

    public class Entity
    {
        public int? Id { get; set; }
        [NotMapped]
        public bool Editable { get => Id.HasValue; }
        [NotMapped]
        public ActionType ActionType { get; set; } 

        public Entity()
        {
            this.ActionType = Editable? ActionType.Update : ActionType.Insert;
        }
      
    }
}
