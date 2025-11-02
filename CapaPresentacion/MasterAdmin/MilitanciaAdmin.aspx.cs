using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CapaEntidad;
using CapaNegocio;
using System.Globalization;
using System.Web.Services;

namespace CapaPresentacion.MasterAdmin
{
	public partial class MilitanciaAdmin : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{

		}

        [WebMethod]
        public static Respuesta<List<EDistrito>> ListaDistritos()
        {
            return NMilitante.GetInstance().ListaDistritos();
        }

        [WebMethod]
        public static Respuesta<List<EMilitante>> ListaMilitantes()
        {
            return NMilitante.GetInstance().ListaMilitantes();
        }

        [WebMethod]
        public static Respuesta<bool> Guardar(EMilitante oPaciente)
        {
            try
            {
                // Intentar parsear la fecha exacta
                DateTime VFechaNacido = DateTime.ParseExact(
                    oPaciente.FechaNacido,
                    "dd/MM/yyyy",
                    CultureInfo.InvariantCulture
                );

                // Crear objeto EPaciente con los datos
                EMilitante obj = new EMilitante
                {
                    NombresCompleto = oPaciente.NombresCompleto,
                    NroCI = oPaciente.NroCI,
                    Celular = oPaciente.Celular,
                    VFechaNacido = VFechaNacido,
                    IdDistrito = oPaciente.IdDistrito
                };

                // Registrar el paciente
                Respuesta<bool> respuesta = NMilitante.GetInstance().RegistrarMilitante(obj);
                return respuesta;
            }
            catch (FormatException)
            {
                return new Respuesta<bool>
                {
                    Estado = false,
                    Mensaje = "La fecha de nacimiento no tiene el formato válido (dd/MM/yyyy)."
                };
            }
            catch (Exception ex)
            {
                // Manejar otras excepciones
                return new Respuesta<bool>
                {
                    Estado = false,
                    Mensaje = "Ocurrió un error: " + ex.Message
                };
            }
        }

        [WebMethod]
        public static Respuesta<bool> Editar(EMilitante oPaciente)
        {
            try
            {
                if (oPaciente == null || oPaciente.IdMilitante <= 0)
                {
                    return new Respuesta<bool>()
                    {
                        Estado = false,
                        Mensaje = "Ocurrió un problema, intente más tarde"
                    };
                }

                // Intentar parsear la fecha en formato dd/MM/yyyy
                DateTime VFechaNacido = DateTime.ParseExact(
                    oPaciente.FechaNacido,
                    "dd/MM/yyyy",
                    CultureInfo.InvariantCulture
                );

                // Crear objeto EPaciente para editar
                EMilitante obj = new EMilitante
                {
                    IdMilitante = oPaciente.IdMilitante,
                    NombresCompleto = oPaciente.NombresCompleto,
                    NroCI = oPaciente.NroCI,
                    Celular = oPaciente.Celular,
                    VFechaNacido = VFechaNacido,
                    IdDistrito = oPaciente.IdDistrito,
                    Estado = oPaciente.Estado
                };

                // Pasar el objeto transformado a la capa de negocio
                Respuesta<bool> respuesta = NMilitante.GetInstance().EditarMilitante(obj);

                return respuesta;
            }
            catch (FormatException)
            {
                return new Respuesta<bool>
                {
                    Estado = false,
                    Mensaje = "La fecha de nacimiento no tiene el formato válido (dd/MM/yyyy)."
                };
            }
            catch (Exception ex)
            {
                return new Respuesta<bool>
                {
                    Estado = false,
                    Mensaje = "Ocurrió un error: " + ex.Message
                };
            }
        }

    }
}