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

    }
}