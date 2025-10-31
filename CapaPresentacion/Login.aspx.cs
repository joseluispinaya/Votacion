using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CapaEntidad;
using CapaNegocio;
using System.Web.Services;

namespace CapaPresentacion
{
	public partial class Login : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{

		}

        [WebMethod]
        public static Respuesta<EDelegado> Logeo(string Usuario, string Clave)
        {
            try
            {
                bool esAdmin = ValidarAdmin(Usuario);
                if (!esAdmin)
                {
                    return new Respuesta<EDelegado>
                    {
                        Estado = false,
                        Valor = "admin",
                        Mensaje = "Ingreso como Administrador"
                    };
                }

                var obj = NDelegado.GetInstance().LoginDelegado(Usuario, Clave);

                if (!obj.Estado)
                {
                    return new Respuesta<EDelegado>
                    {
                        Estado = false,
                        Valor = "",
                        Mensaje = obj.Mensaje
                    };
                }

                var objUser = obj.Data;
                if (!objUser.RefPersona.Estado)
                {
                    return new Respuesta<EDelegado>
                    {
                        Estado = false,
                        Valor = "",
                        Mensaje = "El delegado no se encuentra activo para el uso del sistema."
                    };
                }

                //HttpContext.Current.Session["adminUs"] = objUser;

                return obj;
            }
            catch (Exception ex)
            {
                return new Respuesta<EDelegado>
                {
                    Estado = false,
                    Valor = "",
                    Mensaje = "Ocurrió un error: " + ex.Message
                };
            }
        }

        private static bool ValidarAdmin(string correo)
        {
            try
            {
                var respuesta = NLocalidad.GetInstance().ListaPersonas();

                // Validar que la respuesta sea válida y contenga datos
                if (respuesta == null || respuesta.Data == null || respuesta.Data.Count == 0)
                {
                    return false;
                }

                var listaUsuarios = respuesta.Data;

                var item = listaUsuarios.FirstOrDefault(x => x.Correo.Equals(correo, StringComparison.OrdinalIgnoreCase));
                return item != null; // Devuelve true si encontró el correo
            }
            catch (Exception)
            {
                return false;
            }
        }


        [WebMethod]
        public static Respuesta<EAdministrador> LogeoAdmin(string Usuario, string Clave)
        {
            try
            {
                var obj = NDelegado.GetInstance().LoginAdmin(Usuario, Clave);

                return obj;
            }
            catch (Exception ex)
            {
                return new Respuesta<EAdministrador>
                {
                    Estado = false,
                    Mensaje = "Ocurrió un error: " + ex.Message
                };
            }
        }

    }
}