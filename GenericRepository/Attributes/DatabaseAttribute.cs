using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericRepository.Attributes
{
    // Este atributo permitira asignar los nombres de los storedprocedure para hacer cambios en la base datos
    // ya sea insertar,leer,actualizar,cambio de status, eliminar
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class DatabaseAttribute: Attribute
    {
        private readonly string createProcedureName;
        private readonly string readProcedureName;
        private readonly string updateProcedureName;
        private readonly string changeStatusProcedureName;
        private readonly string deleteProcedureName;

        public DatabaseAttribute(string createProcedureName=null, string readProcedureName=null, string updateProcedureName=null, string changeStatusProcedureName=null,string deleteProcedureName=null)
        {
            this.createProcedureName = createProcedureName;
            this.readProcedureName = readProcedureName;
            this.updateProcedureName = updateProcedureName;
            this.changeStatusProcedureName = changeStatusProcedureName;
            this.deleteProcedureName = deleteProcedureName;
        }
        public string CreateProcedureName { get => createProcedureName; }
        public string ReadProcedureName { get => readProcedureName; }
        public string UpdateProcedureName { get => updateProcedureName; }
        public string ChangeStatusProcedureName { get => changeStatusProcedureName; }
        public string DeleteProcedureName { get => deleteProcedureName; }
    }
}
