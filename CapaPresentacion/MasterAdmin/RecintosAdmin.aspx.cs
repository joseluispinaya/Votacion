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
	public partial class RecintosAdmin : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{

		}

        [WebMethod]
        public static Respuesta<List<ERecinto>> ListaRecintosTotMesas(int IdLocalidad, int IdEleccion)
        {
            return NResultVoto.GetInstance().ListaRecintosTotMesas(IdLocalidad, IdEleccion);
        }

        [WebMethod]
        public static Respuesta<bool> Guardar(ERecinto oRecinto)
        {
            return NResultVoto.GetInstance().RegistrarRecinto(oRecinto);
        }

        [WebMethod]
        public static Respuesta<bool> Editar(ERecinto oRecinto)
        {
            return NResultVoto.GetInstance().EditarRecinto(oRecinto);
        }

    }
}