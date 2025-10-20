using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class NLocalidad
    {
        #region "PATRON SINGLETON"
        private static NLocalidad daoEmpleado = null;
        private NLocalidad() { }
        public static NLocalidad GetInstance()
        {
            if (daoEmpleado == null)
            {
                daoEmpleado = new NLocalidad();
            }
            return daoEmpleado;
        }
        #endregion

        public Respuesta<List<ELocalidad>> ListaLocalidades()
        {
            return DLocalidad.GetInstance().ListaLocalidades();
        }

        public Respuesta<List<ERecinto>> ListaRecintos(int IdLocalidad)
        {
            return DLocalidad.GetInstance().ListaRecintos(IdLocalidad);
        }

        public Respuesta<List<EMesa>> ListaMesas(int IdRecinto)
        {
            return DLocalidad.GetInstance().ListaMesas(IdRecinto);
        }
    }
}
