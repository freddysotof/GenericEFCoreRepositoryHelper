using GenericRepository.Attributes;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GenericRepository.Helpers
{
    // Esta es una clase generica para hacer peticiones a la base de datos por atributos de la clase
    public abstract class StoredProcedureHandler<T> where T: class
    {

        protected DbSet<T> _collections;
        private readonly DbContext _db;
        public StoredProcedureHandler(DbContext dbcontext)
        {
            _db = dbcontext;
            _collections = _db.Set<T>();


        }

        #region Insert Generic Methods/Functions
        // Obtiene los datos necesarios para construir un SqlParameter por medio de los atributos de la propiedad del 
        // modelo
        private static SqlParameter BuildInsertParameterFromPropertyAttributes(object model, PropertyInfo property)
        {
            var propDatabaseParameterAttribute = property.GetCustomAttributes(typeof(InsertParameterAttribute), false);
            if (propDatabaseParameterAttribute.Any())
            {
                var dbAttribute = (InsertParameterAttribute)propDatabaseParameterAttribute.FirstOrDefault();
                string parameterName = dbAttribute.ParameterName ?? $"@{property.Name}";

                //if (dbAttribute.IsIdAttribute && isNew)
                //    parameterName = "@NewId";
                SqlParameter sqlParameter = new()
                {
                    ParameterName = parameterName,
                    SqlDbType = dbAttribute.DataType,
                    Value = property.GetValue(model, null) ?? DBNull.Value,
                    Direction = dbAttribute.ParameterDirection,
                    IsNullable= dbAttribute.IsNullable,
                };
                if (dbAttribute.Size.HasValue)
                    sqlParameter.Size = dbAttribute.Size.Value;
                return sqlParameter;
            }
            return null;
        }

        // Construye una lista de parametros del modelo por medio de los atributos personalizados
        // que tienen las propiedades
        private static List<SqlParameter> BuildInsertModelSqlParameters(object model)
        {
            List<SqlParameter> sqlParameters = new();
            if (model != null)
            {

                foreach (var prop in model?.GetType().GetProperties())
                {
                    var param = BuildInsertParameterFromPropertyAttributes(model, prop);
                    if (param != null)
                        sqlParameters.Add(param);
                }
            }
            return sqlParameters;
        }

        // Obtiene el nombre del parametro que tiene la propiedad como atributo personalizado
        private static string BuildInsertParameterNameFromPropertyAttributes(PropertyInfo property)
        {
            var propDatabaseParameterAttribute = property.GetCustomAttributes(typeof(InsertParameterAttribute), false);
            if (propDatabaseParameterAttribute.Any())
            {
                var dbAttribute = (InsertParameterAttribute)propDatabaseParameterAttribute.FirstOrDefault();
                string parameterName = dbAttribute.ParameterName ?? $"@{property.Name}";
                parameterName += $"{((dbAttribute.IsParameterDirectionOutput) ? " Output" : null)}";
                //if (dbAttribute.IsIdAttribute && isNew)
                //    parameterName = "@NewId";
                return parameterName;
            }
            return null;
        }

        // Construye un string delimitado por comas con todos los parametros del modelo
        // por medio de los atributos personalizados que tienen las propiedades
        private static string BuildInsertModelRawSqlParameters(object model)
        {
            List<string> sqlParameterRaw = new();
            if (model != null)
            {
                foreach (var prop in model?.GetType().GetProperties())
                {
                    string parameterName = BuildInsertParameterNameFromPropertyAttributes(prop);
                    if (parameterName != null)
                        sqlParameterRaw.Add(parameterName);
                }
            }

            return string.Join(",", sqlParameterRaw);
        }

        // Construye la sentencia Sql para insertar en la base de datos.
        public virtual string BuildInsertSqlRawQuery(object model, ProcedureType procedureType)
        {
            string sqlRawParameters = BuildInsertModelRawSqlParameters(model);
            string procedureName = GetProcedureNameFromModelAttribute(model, procedureType);
            return $"exec {procedureName} {sqlRawParameters}";
        }
        #endregion

        #region Update Generic Methods/Functions
        // Obtiene los datos necesarios para construir un SqlParameter por medio de los atributos de la propiedad del 
        // modelo
        private static SqlParameter BuildUpdateParameterFromPropertyAttributes(object model, PropertyInfo property)
        {
            var propDatabaseParameterAttribute = property.GetCustomAttributes(typeof(UpdateParameterAttribute), false);
            if (propDatabaseParameterAttribute.Any())
            {
                var dbAttribute = (UpdateParameterAttribute)propDatabaseParameterAttribute.FirstOrDefault();
                string parameterName = dbAttribute.ParameterName ?? $"@{property.Name}";
              
                //if (dbAttribute.IsIdAttribute && isNew)
                //    parameterName = "@NewId";
                SqlParameter sqlParameter = new()
                {
                    ParameterName = parameterName,
                    SqlDbType = dbAttribute.DataType,
                    Value = property.GetValue(model, null) ?? DBNull.Value,
                    Direction = dbAttribute.ParameterDirection,
                    IsNullable = dbAttribute.IsNullable
                };
                if (dbAttribute.Size.HasValue)
                    sqlParameter.Size = dbAttribute.Size.Value;
                return sqlParameter;
            }
            return null;
        }

        // Construye una lista de parametros del modelo por medio de los atributos personalizados
        // que tienen las propiedades
        private static List<SqlParameter> BuildUpdateModelSqlParameters(object model)
        {
            List<SqlParameter> sqlParameters = new();
            if (model != null)
            {

                foreach (var prop in model?.GetType().GetProperties())
                {
                    var param = BuildUpdateParameterFromPropertyAttributes(model, prop);
                    if (param != null)
                        sqlParameters.Add(param);
                }
            }
            return sqlParameters;
        }

        // Obtiene el nombre del parametro que tiene la propiedad como atributo personalizado
        private static string BuildUpdateParameterNameFromPropertyAttributes(PropertyInfo property)
        {
            var propDatabaseParameterAttribute = property.GetCustomAttributes(typeof(UpdateParameterAttribute), false);
            if (propDatabaseParameterAttribute.Any())
            {
                var dbAttribute = (UpdateParameterAttribute)propDatabaseParameterAttribute.FirstOrDefault();
                string parameterName = dbAttribute.ParameterName ?? $"@{property.Name}";
                parameterName += $"{((dbAttribute.IsParameterDirectionOutput) ? " Output" : null)}";
                //if (dbAttribute.IsIdAttribute && isNew)
                //    parameterName = "@NewId";
                return parameterName;
            }
            return null;
        }

        // Construye un string delimitado por comas con todos los parametros del modelo
        // por medio de los atributos personalizados que tienen las propiedades
        private static string BuildUpdateModelRawSqlParameters(object model)
        {
            List<string> sqlParameterRaw = new();
            if (model != null)
            {
                foreach (var prop in model?.GetType().GetProperties())
                {
                    string parameterName = BuildUpdateParameterNameFromPropertyAttributes(prop);
                    if (parameterName != null)
                        sqlParameterRaw.Add(parameterName);
                }
            }

            return string.Join(",", sqlParameterRaw);
        }

        // Construye la sentencia Sql para insertar en la base de datos.
        public virtual string BuildUpdateSqlRawQuery(object model, ProcedureType procedureType)
        {
            string sqlRawParameters = BuildUpdateModelRawSqlParameters(model);
            string procedureName = GetProcedureNameFromModelAttribute(model, procedureType);
            return $"exec {procedureName} {sqlRawParameters}";
        }
        #endregion

        #region Get By Generic Methods/Functions
        // Obtiene los datos necesarios para construir un SqlParameter de buscar en la base de datos por medio
        // de los atributos del modelo
        private static SqlParameter BuildGetByParameterFromPropertyAttributes(object model, PropertyInfo property)
        {
            var propDatabaseParameterAttribute = property.GetCustomAttributes(typeof(GetByParameterAttribute), false);
            if (propDatabaseParameterAttribute.Any())
            {
                var dbAttribute = (GetByParameterAttribute)propDatabaseParameterAttribute.FirstOrDefault();
                string parameterName = dbAttribute.ParameterName ?? $"@{property.Name}";
                SqlParameter sqlParameter = new()
                {
                    ParameterName = parameterName,
                    SqlDbType = dbAttribute.DataType,
                    Value = property.GetValue(model, null) ?? DBNull.Value,
                    Direction = dbAttribute.ParameterDirection,
                    IsNullable = dbAttribute.IsNullable,
                };
                if (dbAttribute.Size.HasValue)
                    sqlParameter.Size = dbAttribute.Size.Value;
                return sqlParameter;
            }
            return null;
        }
        // Construye una lista de parametros para buscar
        // por medio de los atributos personalizados que tiene el modelo
        public virtual List<SqlParameter> BuildGetByModelSqlParameters(object model)
        {
            List<SqlParameter> sqlParameters = new();
            if (model != null)
            {
                foreach (var prop in model?.GetType().GetProperties())
                {
                    var param = BuildGetByParameterFromPropertyAttributes(model, prop);
                    if (param != null)
                        sqlParameters.Add(param);
                }

            }
            return sqlParameters;
        }

        // Obtiene el nombre del parametro que tiene la propiedad de GetBy como atributo personalizado
        private static string BuildGetByParameterNameFromPropertyAttributes(PropertyInfo property)
        {
            var propDatabaseParameterAttribute = property.GetCustomAttributes(typeof(GetByParameterAttribute), false);
            if (propDatabaseParameterAttribute.Any())
            {
                var dbAttribute = (GetByParameterAttribute)propDatabaseParameterAttribute.FirstOrDefault();
                string parameterName = dbAttribute.ParameterName ?? $"@{property.Name}";
                return parameterName;
            }
            return null;
        }

        // Construye un string delimitado por comas con todos los parametros especificados para buscar en la base de datos
        // por medio del atributo que tiene el modelo
        private static string BuildGetByModelRawSqlParameters(object model)
        {
            List<string> sqlParameterRaw = new();
            if (model != null)
            {
                foreach (var prop in model?.GetType().GetProperties())
                {
                    string parameterName = BuildGetByParameterNameFromPropertyAttributes(prop);
                    if (parameterName != null)
                        sqlParameterRaw.Add(parameterName);
                }
            }

            return string.Join(",", sqlParameterRaw);
        }

        // Construye la sentencia Sql para buscar en la base de datos.
        public virtual string BuildGetBySqlRawQuery(object model, ProcedureType procedureType, bool isNew = false)
        {
            string sqlRawParameters = BuildGetByModelRawSqlParameters(model);
            string procedureName = GetProcedureNameFromModelAttribute(model, procedureType);
            return $"exec {procedureName} {sqlRawParameters}";
        }
        // Construye la sentencia Sql para buscar en la base de datos con el nombre dle procedimiento y los parametros.
        #endregion

        #region Change Status Generic Methods/Functions
        // Obtiene los datos necesarios para construir un SqlParameter cambiar status en la base de datos por medio
        // de los atributos del modelo
        private static SqlParameter BuildChangeStatusParameterFromPropertyAttributes(object model, PropertyInfo property)
        {
            var propDatabaseParameterAttribute = property.GetCustomAttributes(typeof(ChangeStatusParameterAttribute), false);
            if (propDatabaseParameterAttribute.Any())
            {
                var dbAttribute = (ChangeStatusParameterAttribute)propDatabaseParameterAttribute.FirstOrDefault();
                string parameterName = dbAttribute.ParameterName ?? $"@{property.Name}";
              
                SqlParameter sqlParameter = new()
                {
                    ParameterName = parameterName,
                    SqlDbType = dbAttribute.DataType,
                    Value = property.GetValue(model, null) ?? DBNull.Value,
                    Direction = dbAttribute.ParameterDirection,
                    IsNullable = dbAttribute.IsNullable,
                };
                if (dbAttribute.Size.HasValue)
                    sqlParameter.Size = dbAttribute.Size.Value;
                return sqlParameter;
            }
            return null;
        }

        // Construye una lista de parametros para cambiar status
        // por medio de los atributos personalizados que tiene el modelo
        public virtual List<SqlParameter> BuildChangeStatusModelSqlParameters(object model)
        {
            List<SqlParameter> sqlParameters = new();
            if (model != null)
            {
                foreach (var prop in model?.GetType().GetProperties())
                {
                    var param = BuildChangeStatusParameterFromPropertyAttributes(model, prop);
                    if (param != null)
                        sqlParameters.Add(param);
                }

            }
            return sqlParameters;
        }

        // Obtiene el nombre del parametro que tiene la propiedad de change status como atributo personalizado
        private static string BuildChangeStatusParameterNameFromPropertyAttributes(PropertyInfo property )
        {
            var propDatabaseParameterAttribute = property.GetCustomAttributes(typeof(ChangeStatusParameterAttribute), false);
            if (propDatabaseParameterAttribute.Any())
            {
                var dbAttribute = (ChangeStatusParameterAttribute)propDatabaseParameterAttribute.FirstOrDefault();
                string parameterName = dbAttribute.ParameterName ?? $"@{property.Name}";
                parameterName += $"{((dbAttribute.IsParameterDirectionOutput) ? " Output" : null)}";
                return parameterName;
            }
            return null;
        }

        // Construye un string delimitado por comas con todos los parametros especificados para cambiar status en la base de datos
        // por medio del atributo que tiene el modelo
        private static string BuildChangeStatusModelRawSqlParameters(object model)
        {
            List<string> sqlParameterRaw = new();
            if (model != null)
            {
                foreach (var prop in model?.GetType().GetProperties())
                {
                    string parameterName = BuildChangeStatusParameterNameFromPropertyAttributes(prop);
                    if (parameterName != null)
                        sqlParameterRaw.Add(parameterName);
                }
            }

            return string.Join(",", sqlParameterRaw);
        }

        // Construye la sentencia Sql para insertar en la base de datos.
        public virtual string BuildChangeStatusSqlRawQuery(object model, ProcedureType procedureType, bool isNew = false)
        {
            string sqlRawParameters = BuildChangeStatusModelRawSqlParameters(model);
            string procedureName = GetProcedureNameFromModelAttribute(model, procedureType);
            return $"exec {procedureName} {sqlRawParameters}";
        }
        #endregion

        #region Delete Generic Methods/Functions
        // Obtiene los datos necesarios para construir un SqlParameter eliminar en la base de datos por medio
        // de los atributos del modelo
        private static SqlParameter BuildDeleteParameterFromPropertyAttributes(object model, PropertyInfo property)
        {
            var propDatabaseParameterAttribute = property.GetCustomAttributes(typeof(DeleteParameterAttribute), false);
            if (propDatabaseParameterAttribute.Any())
            {
                var dbAttribute = (DeleteParameterAttribute)propDatabaseParameterAttribute.FirstOrDefault();
                string parameterName = dbAttribute.ParameterName ?? $"@{property.Name}";
              
                SqlParameter sqlParameter = new()
                {
                    ParameterName = parameterName,
                    SqlDbType = dbAttribute.DataType,
                    Value = property.GetValue(model, null) ?? DBNull.Value,
                    Direction = dbAttribute.ParameterDirection,
                    IsNullable = dbAttribute.IsNullable,
                };
                if (dbAttribute.Size.HasValue)
                    sqlParameter.Size = dbAttribute.Size.Value;
                return sqlParameter;
            }
            return null;
        }

        // Construye una lista de parametros para eliminar
        // por medio de los atributos personalizados que tiene el modelo
        public virtual List<SqlParameter> BuildDeleteModelSqlParameters(object model)
        {
            List<SqlParameter> sqlParameters = new();
            if (model != null)
            {
                foreach (var prop in model?.GetType().GetProperties())
                {
                    var param = BuildDeleteParameterFromPropertyAttributes(model, prop);
                    if (param != null)
                        sqlParameters.Add(param);
                }

            }
            return sqlParameters;
        }

        // Obtiene el nombre del parametro que tiene la propiedad de change status como atributo personalizado
        private static string BuildDeleteParameterNameFromPropertyAttributes(PropertyInfo property)
        {
            var propDatabaseParameterAttribute = property.GetCustomAttributes(typeof(DeleteParameterAttribute), false);
            if (propDatabaseParameterAttribute.Any())
            {
                var dbAttribute = (DeleteParameterAttribute)propDatabaseParameterAttribute.FirstOrDefault();
                string parameterName = dbAttribute.ParameterName ?? $"@{property.Name}";
                parameterName += $"{((dbAttribute.IsParameterDirectionOutput) ? " Output" : null)}";
                return parameterName;
            }
            return null;
        }

        // Construye un string delimitado por comas con todos los parametros especificados para eliminar en la base de datos
        // por medio del atributo que tiene el modelo
        private static string BuildDeleteModelRawSqlParameters(object model)
        {
            List<string> sqlParameterRaw = new();
            if (model != null)
            {
                foreach (var prop in model?.GetType().GetProperties())
                {
                    string parameterName = BuildDeleteParameterNameFromPropertyAttributes(prop);
                    if (parameterName != null)
                        sqlParameterRaw.Add(parameterName);
                }
            }

            return string.Join(",", sqlParameterRaw);
        }

        // Construye la sentencia Sql para insertar en la base de datos.
        public virtual string BuildDeleteSqlRawQuery(object model, ProcedureType procedureType, bool isNew = false)
        {
            string sqlRawParameters = BuildDeleteModelRawSqlParameters(model);
            string procedureName = GetProcedureNameFromModelAttribute(model, procedureType);
            return $"exec {procedureName} {sqlRawParameters}";
        }
        #endregion

        #region Generic Methods
        // Obtiene los nombres de los procedures que se definieron en los atributos personalizados
        // del modelo. Dependiendo el procedure type se eligira el nombre del procedure (CRUD)
        public string GetProcedureNameFromModelAttribute(object model,ProcedureType procedureType)
        {
            var modelAttributes = model?.GetType().GetCustomAttributes(typeof(DatabaseAttribute), false);
            if (modelAttributes != null && modelAttributes.Any())
            {
                var attributes = (DatabaseAttribute)modelAttributes.FirstOrDefault();
                switch (procedureType)
                {
                    case ProcedureType.Create:
                        return attributes.CreateProcedureName;
                    case ProcedureType.ChangeStatus:
                        return attributes.ChangeStatusProcedureName;
                    case ProcedureType.Update:
                        return attributes.UpdateProcedureName;
                    case ProcedureType.Read:
                        return attributes.ReadProcedureName;
                    case ProcedureType.Delete:
                        return attributes.DeleteProcedureName;
                    case ProcedureType.Other:
                        break;
                    default:
                        return null;
                }
            }
            return null;
        }
        private static string JoinSqlParameters(List<SqlParameter> sqlParameters)
        {
            List<string> parameters = new();
            if(sqlParameters != null && sqlParameters.Any())
                foreach (var param in sqlParameters)
                {
                    var parameterName = param.ParameterName;
                    parameterName += $"{((param.Direction == ParameterDirection.Output) ? " Output" : null)}";
                    parameters.Add(parameterName);
                }
            return string.Join(",", parameters);
        }

        public virtual string BuildSqlRawQuery(string procedureName, List<SqlParameter> sqlParameters)
        {
            return $"exec {procedureName} {JoinSqlParameters(sqlParameters)} ";
        }

        // ejecuta la sentencia Sql por medio del DbSet, modelo especificido por atributos y el nombre del procedimiento
        public virtual Task<List<T>> ExecuteFromSqlRaw(object model, ProcedureType procedureType, string procedureName = null)
        {
            
            if(string.IsNullOrEmpty(procedureName))
                procedureName = GetProcedureNameFromModelAttribute(model, procedureType);
            var parameters = BuildGetByModelRawSqlParameters(model);
            var parametersArray = BuildGetByModelSqlParameters(model).ToArray();
            var result = _collections.FromSqlRaw(
                                        $"exec {procedureName} {parameters}",
                                        parametersArray
                                        );
            return Task.FromResult(result.ToList());
        }
        // ejecuta la sentencia Sql por medio del DbSet, nombre del procedimiento y lista de parametros
        public virtual Task<List<T>> ExecuteFromSqlRaw(string procedureName, List<SqlParameter> sqlParameters = null)
        {
            List<T> result;
            var parameters = JoinSqlParameters(sqlParameters);
            var parametersArray = sqlParameters?.ToArray();
            if (sqlParameters == null)
                result = _collections.FromSqlRaw($"exec {procedureName}").ToList();
            else
                result = _collections.FromSqlRaw($"exec {procedureName} {parameters}", parametersArray)
                  .ToList();
           
            return Task.FromResult(result);
        }
        // Ejecuta la sentencia Sql por medio del DbContext, el modelo y el tipo de procedimiento  
        public async virtual Task<IEnumerable<SqlParameter>> ExecuteSqlAsync(object model, ProcedureType procedureType)
        {
            //using var transaction = _db.Database.BeginTransaction();
            string sqlRawQuery = null;
            List<SqlParameter> sqlParameters = null;
            //try
            //{
                switch (procedureType)
                {
                    case ProcedureType.Create:
                        sqlRawQuery = BuildInsertSqlRawQuery(model, procedureType);
                        sqlParameters = BuildInsertModelSqlParameters(model);
                        break;
                    case ProcedureType.Update:
                        sqlRawQuery = BuildUpdateSqlRawQuery(model, procedureType);
                        sqlParameters = BuildUpdateModelSqlParameters(model);
                        break;
                    case ProcedureType.ChangeStatus:
                        sqlRawQuery = BuildChangeStatusSqlRawQuery(model, procedureType);
                        sqlParameters = BuildChangeStatusModelSqlParameters(model);
                        break;
                    case ProcedureType.Delete:
                        sqlRawQuery = BuildDeleteSqlRawQuery(model, procedureType);
                        sqlParameters = BuildDeleteModelSqlParameters(model);
                        break;
                }
                var result = await _db.Database.ExecuteSqlRawAsync(sqlRawQuery, sqlParameters);
                //transaction.Commit();
                //_db.Entry(_collections).State = EntityState.Detached;
            //}
            //catch (Exception e)
            //{
            //    //transaction.Rollback();
            //}
            return sqlParameters?.Where(x => x.Direction == ParameterDirection.Output);
        }

        // Ejecuta la sentencia Sql por medio del DbContext, el modelo y el tipo de procedimiento usando transaction 
        public async virtual Task<IEnumerable<SqlParameter>> ExecuteSqlTransactionAsync(object model, ProcedureType procedureType)
        {
            using var transaction = _db.Database.BeginTransaction();
            string sqlRawQuery = null;
            List<SqlParameter> sqlParameters = null;
            try
            {
                switch (procedureType)
                {
                    case ProcedureType.Create:
                        sqlRawQuery = BuildInsertSqlRawQuery(model, procedureType);
                        sqlParameters = BuildInsertModelSqlParameters(model);
                        break;
                    case ProcedureType.Update:
                        sqlRawQuery = BuildUpdateSqlRawQuery(model, procedureType);
                        sqlParameters = BuildUpdateModelSqlParameters(model);
                        break;
                    case ProcedureType.ChangeStatus:
                        sqlRawQuery = BuildChangeStatusSqlRawQuery(model, procedureType);
                        sqlParameters = BuildChangeStatusModelSqlParameters(model);
                        break;
                    case ProcedureType.Delete:
                        sqlRawQuery = BuildDeleteSqlRawQuery(model, procedureType);
                        sqlParameters = BuildDeleteModelSqlParameters(model);
                        break;
                }
                var result = await _db.Database.ExecuteSqlRawAsync(sqlRawQuery, sqlParameters);
                transaction.Commit();
                //_db.Entry(_collections).State = EntityState.Detached;
            }
            catch (Exception e)
            {
                transaction.Rollback();
            }
            return sqlParameters?.Where(x => x.Direction == ParameterDirection.Output);
        }
        // Ejecuta la sentencia Sql por medio del DbContext, nombre del procedure y parametros personalizados 
        public async virtual Task<IEnumerable<SqlParameter>> ExecuteSqlAsync(string procedureName = null,List <SqlParameter> sqlParameters=null)
        {
            //using var transaction = _db.Database.BeginTransaction();
            //try
            //{
                string sqlRaw = $"{procedureName} {JoinSqlParameters(sqlParameters)}";
                //int response;
                if (sqlParameters == null)
                    await _db.Database.ExecuteSqlRawAsync(sqlRaw);
                else
                    await _db.Database.ExecuteSqlRawAsync(sqlRaw, sqlParameters);
                //_db.Entry(_collections).State = EntityState.Detached;
                //transaction.Commit();
            //}
            //catch (Exception e)
            //{
            //    //transaction.Rollback();
            //}
            return sqlParameters?.Where(x => x.Direction == ParameterDirection.Output);
        }
        // Ejecuta la sentencia Sql por medio del DbContext, nombre del procedure y parametros personalizados usando transaction 
        public async virtual Task<IEnumerable<SqlParameter>> ExecuteSqlTransactionAsync(string procedureName = null, List<SqlParameter> sqlParameters = null)
        {
            using var transaction = _db.Database.BeginTransaction();
            try
            {
                string sqlRaw = $"{procedureName} {JoinSqlParameters(sqlParameters)}";
                //int response;
                if (sqlParameters == null)
                    await _db.Database.ExecuteSqlRawAsync(sqlRaw);
                else
                    await _db.Database.ExecuteSqlRawAsync(sqlRaw, sqlParameters);
                //_db.Entry(_collections).State = EntityState.Detached;
                transaction.Commit();
            }
            catch (Exception e)
            {
                transaction.Rollback();
            }
            return sqlParameters?.Where(x => x.Direction == ParameterDirection.Output);
        }
        #endregion

        //protected async void Dispose(bool disposing)
        //{
        //    if (!this.disposed)
        //    {
        //        if (disposing)
        //        {
        //            await _db.DisposeAsync();
        //        }
        //    }
        //    this.disposed = true;
        //}

        //public void Dispose()
        //{
        //    Dispose(true);
        //    GC.SuppressFinalize(this);
        //}

        public enum ProcedureType
        {
            Create=1,
            Read=2,
            Update=3,
            ChangeStatus=4,
            Delete=5,
            Other=6
        }
    }
}