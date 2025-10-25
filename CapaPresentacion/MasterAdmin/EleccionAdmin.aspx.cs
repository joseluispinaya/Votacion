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
	public partial class EleccionAdmin : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{

		}


        [WebMethod]
        public static Respuesta<List<ETipoEleccion>> ListaTiposEle()
        {
            try
            {
                Respuesta<List<ETipoEleccion>> Lista = NLocalidad.GetInstance().ListaTiposEle();
                return Lista;
            }
            catch (Exception ex)
            {
                // Maneja cualquier error inesperado
                return new Respuesta<List<ETipoEleccion>>()
                {
                    Estado = false,
                    Mensaje = "Error al obtener los ETipoEleccion: " + ex.Message,
                    Data = null
                };
            }
        }

        [WebMethod]
        public static Respuesta<List<EEleccion>> ListaElecciones()
        {
            try
            {
                Respuesta<List<EEleccion>> Lista = NLocalidad.GetInstance().ListaElecciones();
                return Lista;
            }
            catch (Exception ex)
            {
                // Maneja cualquier error inesperado
                return new Respuesta<List<EEleccion>>()
                {
                    Estado = false,
                    Mensaje = "Error al obtener los EEleccion: " + ex.Message,
                    Data = null
                };
            }
        }

        [WebMethod]
        public static Respuesta<bool> Guardar(EEleccion oEleccion)
        {
            try
            {
                // Registrar
                Respuesta<bool> respuesta = NLocalidad.GetInstance().RegistrarEleccion(oEleccion);
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