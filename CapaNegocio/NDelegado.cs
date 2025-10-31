using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class NDelegado
    {
        #region "PATRON SINGLETON"
        private static NDelegado daoEmpleado = null;
        private NDelegado() { }
        public static NDelegado GetInstance()
        {
            if (daoEmpleado == null)
            {
                daoEmpleado = new NDelegado();
            }
            return daoEmpleado;
        }
        #endregion

        public Respuesta<bool> RegistrarDelegadosNuevo(EDelegado oDelegado)
        {
            return DDelegado.GetInstance().RegistrarDelegadosNuevo(oDelegado);
        }

        public Respuesta<bool> RegistrarDelegados(EDelegado oDelegado)
        {
            return DDelegado.GetInstance().RegistrarDelegados(oDelegado);
        }

        public Respuesta<List<EMesa>> ListaMesasSelect(int IdRecinto, int IdEleccion)
        {
            return DDelegado.GetInstance().ListaMesasSelect(IdRecinto, IdEleccion);
        }

        public Respuesta<List<EMesaDto>> ListaMesasDelegados(int IdRecinto, int IdEleccion)
        {
            return DDelegado.GetInstance().ListaMesasDelegados(IdRecinto, IdEleccion);
        }

        public Respuesta<List<EListaMesasDto>> MesasAsignadasDelegados(int IdPersona, int IdEleccion)
        {
            return DDelegado.GetInstance().MesasAsignadasDelegados(IdPersona, IdEleccion);
        }
        public Respuesta<EDelegado> LoginDelegado(string correo, string claveHash)
        {
            return DDelegado.GetInstance().LoginDelegado(correo, claveHash);
        }
        public Respuesta<EAdministrador> LoginAdmin(string correo, string claveHash)
        {
            return DDelegado.GetInstance().LoginAdmin(correo, claveHash);
        }

        public Respuesta<List<EPartidoPol>> ListaPartidosVota(int IdEleccion, int IdMesa)
        {
            return DDelegado.GetInstance().ListaPartidosVota(IdEleccion, IdMesa);
        }

    }
}
