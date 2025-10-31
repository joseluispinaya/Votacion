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
	public partial class PersonasAdmin : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{

		}

        [WebMethod]
        public static Respuesta<List<EPersona>> ObtenerPersonasFiltro(string busqueda)
        {
            try
            {
                var Lista = NLocalidad.GetInstance().ObtenerPersonasFiltro(busqueda);
                return Lista;
            }
            catch (Exception ex)
            {
                // Maneja cualquier error inesperado
                return new Respuesta<List<EPersona>>()
                {
                    Estado = false,
                    Mensaje = "Error al obtener los EPersona: " + ex.Message,
                    Data = null
                };
            }

        }

        [WebMethod]
        public static Respuesta<List<EPersona>> ListaPersonas()
        {
            try
            {
                Respuesta<List<EPersona>> Lista = NLocalidad.GetInstance().ListaPersonas();
                return Lista;
            }
            catch (Exception ex)
            {
                // Maneja cualquier error inesperado
                return new Respuesta<List<EPersona>>()
                {
                    Estado = false,
                    Mensaje = "Error al obtener los EPersona: " + ex.Message,
                    Data = null
                };
            }
        }

        [WebMethod]
        public static Respuesta<bool> Guardar(EPersona oPersona)
        {
            try
            {
                // Registrar
                Respuesta<bool> respuesta = NLocalidad.GetInstance().RegistrarPersona(oPersona);
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