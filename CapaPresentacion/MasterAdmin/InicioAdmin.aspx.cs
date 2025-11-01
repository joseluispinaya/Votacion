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
	public partial class InicioAdmin : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{

		}

        // no se usa
        [WebMethod]
        public static Respuesta<List<EResultadoGeneral>> ResultadoGeneralVotacion(int IdEleccion)
        {
            try
            {
                if (IdEleccion <= 0)
                {
                    return new Respuesta<List<EResultadoGeneral>>() { Estado = false, Mensaje = "Ocurio un error" };
                }

                Respuesta<List<EResultadoGeneral>> Lista = NResultVoto.GetInstance().ResultadoGeneralVotacion(IdEleccion);
                return Lista;
            }
            catch (Exception ex)
            {
                // Maneja cualquier error inesperado
                return new Respuesta<List<EResultadoGeneral>>()
                {
                    Estado = false,
                    Mensaje = "Error al obtener los resultados: " + ex.Message
                };
            }
        }

        [WebMethod]
        public static Respuesta<List<EResultadoGeneral>> ResultGeneVotacionNuevo()
        {
            return NResultVoto.GetInstance().ResultGeneVotacionNuevo();
        }
    }
}