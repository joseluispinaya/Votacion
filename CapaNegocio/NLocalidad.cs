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

        public Respuesta<List<EMesa>> ListaMesas(int IdRecinto, int IdEleccion)
        {
            return DLocalidad.GetInstance().ListaMesas(IdRecinto, IdEleccion);
        }
        public Respuesta<List<ETipoEleccion>> ListaTiposEle()
        {
            return DLocalidad.GetInstance().ListaTiposEle();
        }
        public Respuesta<List<EEleccion>> ListaElecciones()
        {
            return DLocalidad.GetInstance().ListaElecciones();
        }

        public Respuesta<bool> RegistrarEleccion(EEleccion oEleccion)
        {
            return DLocalidad.GetInstance().RegistrarEleccion(oEleccion);
        }
        public Respuesta<bool> RegistrarMesas(EMesa oMesa)
        {
            return DLocalidad.GetInstance().RegistrarMesas(oMesa);
        }

        public Respuesta<bool> RegistrarPersona(EPersona oPersona)
        {
            return DLocalidad.GetInstance().RegistrarPersona(oPersona);
        }

        public Respuesta<List<EPersona>> ListaPersonas()
        {
            return DLocalidad.GetInstance().ListaPersonas();
        }

        public Respuesta<List<EPersona>> ObtenerPersonasFiltro(string Busqueda)
        {
            return DLocalidad.GetInstance().ObtenerPersonasFiltro(Busqueda);
        }

    }
}
