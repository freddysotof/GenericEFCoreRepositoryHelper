using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericRepository.Attributes
{
    // Este atributo permitira asignar las propiedades que se utilizaran de la clase para hacer
    // un cambio de status en la base de datos por medio a un storedprocedure
    // Se necesita el tipo de dato de sql
    // el nombre del parametro (en caso de ser diferente al nombre de la propiedad,
    // y el tipo de parametro (ya se input, output o input/output)
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class ChangeStatusParameterAttribute : Attribute
    {
        private readonly string databaseName;
        private readonly ParameterDirection parameterDirection = ParameterDirection.Input;
        private readonly SqlDbType dataType;
        private readonly int? size = 0;
        private bool isNullable = false;
        public ChangeStatusParameterAttribute(SqlDbType dataType)
        {
            this.dataType = dataType;
        }
        public ChangeStatusParameterAttribute(SqlDbType dataType, string databaseName = null,ParameterDirection parameterDirection = ParameterDirection.Input, int size = 0,bool isNullable=false)
        {
            this.databaseName = databaseName;
            this.parameterDirection = parameterDirection;
            this.dataType = dataType;
            this.parameterDirection = parameterDirection;
            if (size > 0)
                this.size = size;
            else
                this.size = null;
            this.isNullable = isNullable;
        }
        public ChangeStatusParameterAttribute(SqlDbType dataType, ParameterDirection parameterDirection = ParameterDirection.Input, bool isNullable = false)
        {
            this.parameterDirection = parameterDirection;
            this.dataType = dataType;
            this.isNullable = isNullable;
        }
        public ChangeStatusParameterAttribute(SqlDbType dataType, bool isNullable = false)
        {
            this.dataType = dataType;
            this.isNullable = isNullable;
        }
        public bool HasDifferentDatabaseName { get => databaseName != null; }
        public string DatabaseName { get => databaseName; }
        public ParameterDirection ParameterDirection { get => parameterDirection; }
        public bool IsParameterDirectionOutput { get => parameterDirection == ParameterDirection.Output; }
        public string ParameterName { get => databaseName != null ? $"@{databaseName}" : null; }
        public int? Size { get => size; }
        public bool IsNullable { get => isNullable; }
        public SqlDbType DataType { get => dataType; }
        public bool IsIdAttribute { get => HasDifferentDatabaseName && DatabaseName.Trim().ToLower() == "id"; }
    }
}
