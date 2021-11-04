using GenericRepositoryHelper.CustomAttributes.Database;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Repository.Models
{
    [Database("dbo.SP_InsertLog", "dbo.SP_GetLogs", "dbo.SP_UpdateLog", "dbo.SP_UpdateLogStatus", "dbo.SP_DeleteLog")]
    public class Log
    {
        [InsertParameter(SqlDbType.Int, null, ParameterDirection.Output)]
        [UpdateParameter(SqlDbType.Int)]
        [GetByParameter(SqlDbType.Int)]
        [ChangeStatusParameter(SqlDbType.Int)]
        [DeleteParameter(SqlDbType.Int)]
        public int? LogId { get; set; }

        [InsertParameter(SqlDbType.DateTime)]
        [UpdateParameter(SqlDbType.DateTime)]
        public DateTime Date { get; set; } = DateTime.Now;

        [InsertParameter(SqlDbType.Int)]
        [UpdateParameter(SqlDbType.Int)]
        public int Thread { get; set; }

        [InsertParameter(SqlDbType.VarChar)]
        [UpdateParameter(SqlDbType.VarChar)]
        public string Level { get; set; }

        [InsertParameter(SqlDbType.VarChar)]
        [UpdateParameter(SqlDbType.VarChar)]
        public string Url { get; set; }

        [InsertParameter(SqlDbType.VarChar)]
        [UpdateParameter(SqlDbType.VarChar)]
        public string Logger { get; set; }

        [InsertParameter(SqlDbType.VarChar)]
        [UpdateParameter(SqlDbType.VarChar)]
        public string Action { get; set; }

        [InsertParameter(SqlDbType.VarChar)]
        [UpdateParameter(SqlDbType.VarChar)]
        public string SourceAction { get; set; }

        [InsertParameter(SqlDbType.VarChar)]
        [UpdateParameter(SqlDbType.VarChar)]
        public string Message { get; set; }

        [InsertParameter(SqlDbType.VarChar)]
        [UpdateParameter(SqlDbType.VarChar)]
        public string Exception { get; set; }

        [InsertParameter(SqlDbType.VarChar,null,ParameterDirection.Output,100)]
        [UpdateParameter(SqlDbType.VarChar)]
        public string App { get; set; }

        [InsertParameter(SqlDbType.VarChar)]
        [UpdateParameter(SqlDbType.VarChar)]
        public string User { get; set; }

        [InsertParameter(SqlDbType.VarChar)]
        [UpdateParameter(SqlDbType.VarChar)]
        public string RequestBody { get; set; }

        [InsertParameter(SqlDbType.VarChar)]
        [UpdateParameter(SqlDbType.VarChar)]
        public string Reference { get; set; }

        [InsertParameter(SqlDbType.VarChar)]
        [UpdateParameter(SqlDbType.VarChar)]
        public string Json { get; set; }

        [ChangeStatusParameter(SqlDbType.Int)]
        public int? StatusId { get; set; }

        public override string ToString()
            => $"Log: ({nameof(LogId)}: {LogId};{nameof(Logger)}: {Logger}; {nameof(Action)}: {Action}; )";
        public override bool Equals(object obj)
        {
            if (obj is Log other)
                return LogId.Equals(other.LogId);
            else
                return false;
        }
        public override int GetHashCode() => base.GetHashCode();
    }
}
