using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericRepository.Extensions
{
    public static class GenericRepositoryExtensions
    {
        public static string JoinSqlParameters(this List<SqlParameter> sqlParameters)
        {
            List<string> parameters = new();
            if (sqlParameters != null && sqlParameters.Any())
                foreach (var param in sqlParameters)
                {
                    var parameterName = param.ParameterName;
                    parameterName += $"{((param.Direction == ParameterDirection.Output) ? " Output" : null)}";
                    parameters.Add(parameterName);
                }
            return string.Join(",", parameters);
            //return string.Join(",", parameters);
        }
    }
}
