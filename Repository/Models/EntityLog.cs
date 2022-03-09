using GenericRepository.Attributes;
using GenericRepository.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Repository.Models
{
   public class EntityLog:IEntity
    {
        public int? Id { get; set; }

        //public int? LogId { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public int Thread { get; set; }

        public string Level { get; set; }

        public string Url { get; set; }
        public string Logger { get; set; }
        public string Action { get; set; }
        public string SourceAction { get; set; }
        public string Message { get; set; }
        public string Exception { get; set; }
        public string App { get; set; }
        public string User { get; set; }
        public string RequestBody { get; set; }

        public string Reference { get; set; }
        public string Json { get; set; }
        public int? StatusId { get; set; }
        public string CreatedBy { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DateTime CreatedDate { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string UpdatedBy { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DateTime? UpdatedDate { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
