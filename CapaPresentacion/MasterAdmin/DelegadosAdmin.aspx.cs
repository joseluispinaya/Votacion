using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CapaEntidad;
using CapaNegocio;
using System.Web.Services;

namespace CapaPresentacion.MasterAdmin
{
	public partial class DelegadosAdmin : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{

		}

        [WebMethod]
        public static Respuesta<List<EMesa>> ListaMesasSelect(int IdRecinto, int IdEleccion)
        {
            try
            {
                if (IdRecinto <= 0 || IdEleccion <= 0)
                {
                    return new Respuesta<List<EMesa>>() { Estado = false, Mensaje = "Debe seleccionar un Recinto y una Eleccion" };
                }

                Respuesta<List<EMesa>> Lista = NDelegado.GetInstance().ListaMesasSelect(IdRecinto, IdEleccion);
                return Lista;
            }
            catch (Exception ex)
            {
                // Maneja cualquier error inesperado
                return new Respuesta<List<EMesa>>()
                {
                    Estado = false,
                    Mensaje = "Error al obtener las Mesas: " + ex.Message,
                    Data = null
                };
            }
        }

        [WebMethod]
        public static Respuesta<List<EMesaDto>> ListaMesasDelegados(int IdRecinto, int IdEleccion)
        {
            try
            {
                if (IdRecinto <= 0 || IdEleccion <= 0)
                {
                    return new Respuesta<List<EMesaDto>>() { Estado = false, Mensaje = "Debe seleccionar un Recinto y una Eleccion" };
                }

                Respuesta<List<EMesaDto>> Lista = NDelegado.GetInstance().ListaMesasDelegados(IdRecinto, IdEleccion);
                return Lista;
            }
            catch (Exception ex)
            {
                // Maneja cualquier error inesperado
                return new Respuesta<List<EMesaDto>>()
                {
                    Estado = false,
                    Mensaje = "Error al obtener las Mesas: " + ex.Message,
                    Data = null
                };
            }
        }

        [WebMethod]
        public static Respuesta<bool> Guardar(EDelegado oDelegado)
        {
            try
            {
                // Registrar
                //Respuesta<bool> respuesta = NDelegado.GetInstance().RegistrarDelegados(oDelegado);
                Respuesta<bool> respuesta = NDelegado.GetInstance().RegistrarDelegadosNuevo(oDelegado);
                return respuesta;
            }
            catch (Exception)
            {
                // Manejar otras excepciones
                return new Respuesta<bool>
                {
                    Estado = false,
                    Mensaje = "Ocurrió un error intente mas tarde"
                };
            }
        }

    }
}