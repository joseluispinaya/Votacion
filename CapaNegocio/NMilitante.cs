using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class NMilitante
    {
        #region "PATRON SINGLETON"
        private static NMilitante daoEmpleado = null;
        private NMilitante() { }
        public static NMilitante GetInstance()
        {
            if (daoEmpleado == null)
            {
                daoEmpleado = new NMilitante();
            }
            return daoEmpleado;
        }
        #endregion

        public Respuesta<List<EDistrito>> ListaDistritos()
        {
            return DMilitante.GetInstance().ListaDistritos();
        }

        public Respuesta<bool> RegistrarMilitante(EMilitante oPersona)
        {
            return DMilitante.GetInstance().RegistrarMilitante(oPersona);
        }
        public Respuesta<bool> EditarMilitante(EMilitante oPersona)
        {
            return DMilitante.GetInstance().EditarMilitante(oPersona);
        }

        public Respuesta<List<EMilitante>> ListaMilitantes()
        {
            return DMilitante.GetInstance().ListaMilitantes();
        }

    }
}
