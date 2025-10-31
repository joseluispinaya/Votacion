using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CapaEntidad;
using CapaNegocio;
using System.Web.Services;
using System.Xml.Linq;

namespace CapaPresentacion
{
	public partial class Inicio : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{

		}

        [WebMethod]
        public static Respuesta<List<EListaMesasDto>> MesasAsignadasDelegados(int IdPersona, int IdEleccion)
        {
            try
            {
                if (IdPersona <= 0 || IdEleccion <= 0)
                {
                    return new Respuesta<List<EListaMesasDto>>() { Estado = false, Mensaje = "Ocurio un error" };
                }

                Respuesta<List<EListaMesasDto>> Lista = NDelegado.GetInstance().MesasAsignadasDelegados(IdPersona, IdEleccion);
                return Lista;
            }
            catch (Exception ex)
            {
                // Maneja cualquier error inesperado
                return new Respuesta<List<EListaMesasDto>>()
                {
                    Estado = false,
                    Mensaje = "Error al obtener las Mesas: " + ex.Message,
                    Data = null
                };
            }
        }

        [WebMethod]
        public static Respuesta<List<EPartidoPol>> ListaPartidosVota(int IdEleccion, int IdMesa)
        {
            try
            {
                if (IdEleccion <= 0 || IdMesa <= 0)
                {
                    return new Respuesta<List<EPartidoPol>>() { Estado = false, Mensaje = "Ocurio un error" };
                }

                Respuesta<List<EPartidoPol>> Lista = NDelegado.GetInstance().ListaPartidosVota(IdEleccion, IdMesa);
                return Lista;
            }
            catch (Exception ex)
            {
                // Maneja cualquier error inesperado
                return new Respuesta<List<EPartidoPol>>()
                {
                    Estado = false,
                    Mensaje = "Error al obtener las Partidos: " + ex.Message,
                    Data = null
                };
            }
        }

        //no se usa muy complejo
        [WebMethod]
        public static Respuesta<bool> GuardarVotos(int IdEleccion, int IdMesa, int IdDelegado, int Nulos, int Blancos, List<EResultado> ListaResultados)
        {
            try
            {
                if (ListaResultados == null || !ListaResultados.Any())
                {
                    return new Respuesta<bool> { Estado = false, Mensaje = "No se encontro datos de votacion de los partidos" };
                }

                Respuesta<bool> respuesta = NResultVoto.GetInstance().GuardarVotos(IdEleccion, IdMesa, IdDelegado, Nulos, Blancos, ListaResultados);

                return respuesta;
            }
            catch (Exception ex)
            {
                return new Respuesta<bool> { Estado = false, Mensaje = "Ocurrió un error: " + ex.Message };
            }
        }

        [WebMethod]
        public static Respuesta<bool> GuardarVotosNuevo(int IdEleccion, int IdMesa, int IdDelegado, int Nulos, int Blancos, List<EResultado> ListaResultados)
        {
            try
            {
                if (ListaResultados == null || !ListaResultados.Any())
                {
                    return new Respuesta<bool> { Estado = false, Mensaje = "No se encontró datos de votación de los partidos." };
                }

                // Construcción del XML
                XElement xml = new XElement("Votacion",

                    new XElement("Especial",
                        new XElement("IdEleccion", IdEleccion),
                        new XElement("IdMesa", IdMesa),
                        new XElement("IdDelegado", IdDelegado),
                        new XElement("Nulos", Nulos),
                        new XElement("Blancos", Blancos)
                    ),

                    new XElement("Resultados",
                        from item in ListaResultados
                        select new XElement("Item",
                            new XElement("IdPartido", item.IdPartido),
                            new XElement("Votos", item.Votos)
                        )
                    )
                );

                // Enviar XML a la capa negocio
                Respuesta<bool> respuesta = NResultVoto.GetInstance().GuardarVotosNuevo(xml.ToString());
                return respuesta;
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