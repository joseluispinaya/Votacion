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
	public partial class LocalidadesAdmin : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{

		}

        [WebMethod]
        public static Respuesta<List<ELocalidad>> ListaLocalidades()
        {
            try
            {
                Respuesta<List<ELocalidad>> Lista = NLocalidad.GetInstance().ListaLocalidades();
                return Lista;
            }
            catch (Exception ex)
            {
                // Maneja cualquier error inesperado
                return new Respuesta<List<ELocalidad>>()
                {
                    Estado = false,
                    Mensaje = "Error al obtener los ELocalidad: " + ex.Message,
                    Data = null
                };
            }
        }

        [WebMethod]
        public static Respuesta<List<ERecinto>> ListaRecintos(int IdLocalidad)
        {
            try
            {
                Respuesta<List<ERecinto>> Lista = NLocalidad.GetInstance().ListaRecintos(IdLocalidad);
                return Lista;
            }
            catch (Exception ex)
            {
                // Maneja cualquier error inesperado
                return new Respuesta<List<ERecinto>>()
                {
                    Estado = false,
                    Mensaje = "Error al obtener los ERecinto: " + ex.Message,
                    Data = null
                };
            }
        }

    }
}