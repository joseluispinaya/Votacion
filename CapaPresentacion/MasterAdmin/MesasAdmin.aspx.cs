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
	public partial class MesasAdmin : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{

		}

        [WebMethod]
        public static Respuesta<List<EMesa>> ListaMesas(int IdRecinto, int IdEleccion)
        {
            try
            {
                if (IdRecinto <= 0 || IdEleccion <= 0)
                {
                    return new Respuesta<List<EMesa>>() { Estado = false, Mensaje = "Debe seleccionar un Recinto y una Eleccion" };
                }

                Respuesta<List<EMesa>> Lista = NLocalidad.GetInstance().ListaMesas(IdRecinto, IdEleccion);
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
        public static Respuesta<bool> Guardar(EMesa oMesa)
        {
            try
            {
                // Registrar
                Respuesta<bool> respuesta = NLocalidad.GetInstance().RegistrarMesas(oMesa);
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