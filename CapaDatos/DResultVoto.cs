using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using CapaEntidad;

namespace CapaDatos
{
    public class DResultVoto
    {
        #region "PATRON SINGLETON"
        public static DResultVoto _instancia = null;

        private DResultVoto()
        {

        }

        public static DResultVoto GetInstance()
        {
            if (_instancia == null)
            {
                _instancia = new DResultVoto();
            }
            return _instancia;
        }
        #endregion

        public Respuesta<bool> GuardarVotos(int IdEleccion, int IdMesa, int IdDelegado, int Nulos, int Blancos, List<EResultado> ListaResultados)
        {
            Respuesta<bool> respuesta = new Respuesta<bool>();

            try
            {
                using (SqlConnection con = ConexionBD.GetInstance().ConexionDB())
                {
                    con.Open();
                    SqlTransaction trans = con.BeginTransaction();

                    try
                    {
                        bool insercionExitosa = true;

                        // Guardar votos por partido
                        foreach (var item in ListaResultados)
                        {
                            using (SqlCommand cmd = new SqlCommand("RegistrarResultado", con, trans))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;

                                cmd.Parameters.AddWithValue("@IdEleccion", IdEleccion);
                                cmd.Parameters.AddWithValue("@IdMesa", IdMesa);
                                cmd.Parameters.AddWithValue("@IdPartido", item.IdPartido);
                                cmd.Parameters.AddWithValue("@Votos", item.Votos);
                                cmd.Parameters.AddWithValue("@IdDelegado", IdDelegado);

                                SqlParameter resultado = new SqlParameter("@Resultado", SqlDbType.Bit)
                                {
                                    Direction = ParameterDirection.Output
                                };
                                cmd.Parameters.Add(resultado);

                                cmd.ExecuteNonQuery();

                                // Verifica OUTPUT del SP
                                if (!(bool)resultado.Value)
                                    insercionExitosa = false;
                            }
                        }

                        // Guardar votos especiales (nulos y blancos)
                        using (SqlCommand cmd = new SqlCommand("RegistrarVotoEspecial", con, trans))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@IdEleccion", IdEleccion);
                            cmd.Parameters.AddWithValue("@IdMesa", IdMesa);
                            cmd.Parameters.AddWithValue("@Nulos", Nulos);
                            cmd.Parameters.AddWithValue("@Blancos", Blancos);
                            cmd.Parameters.AddWithValue("@IdDelegado", IdDelegado);

                            SqlParameter resultado = new SqlParameter("@Resultado", SqlDbType.Bit)
                            {
                                Direction = ParameterDirection.Output
                            };
                            cmd.Parameters.Add(resultado);

                            cmd.ExecuteNonQuery();

                            if (!(bool)resultado.Value)
                                insercionExitosa = false;
                        }

                        // Confirmar o revertir transacción
                        if (insercionExitosa)
                        {
                            trans.Commit();
                            respuesta.Estado = true;
                            respuesta.Mensaje = "Se registraron los votos correctamente.";
                        }
                        else
                        {
                            trans.Rollback();
                            respuesta.Estado = false;
                            respuesta.Mensaje = "Ya existe un registro para esta mesa.";
                        }
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        respuesta.Estado = false;
                        respuesta.Mensaje = $"Ocurrió un error: {ex.Message}";
                    }
                }
            }
            catch (Exception ex)
            {
                respuesta.Estado = false;
                respuesta.Mensaje = $"Error al conectar a BD: {ex.Message}";
            }

            return respuesta;
        }

        public Respuesta<bool> GuardarVotosNuevo(string VotosXml)
        {
            var respuesta = new Respuesta<bool>();

            try
            {
                using (SqlConnection con = ConexionBD.GetInstance().ConexionDB())
                using (SqlCommand cmd = new SqlCommand("usp_RegistrarDatosVotacion", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@VotosXml", SqlDbType.Xml).Value = VotosXml;

                    var resultado = new SqlParameter("@Resultado", SqlDbType.Bit)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(resultado);

                    con.Open();
                    cmd.ExecuteNonQuery();

                    respuesta.Estado = Convert.ToBoolean(resultado.Value);
                    respuesta.Mensaje = respuesta.Estado
                        ? "Registro realizado correctamente."
                        : "Esta mesa ya tiene votos registrados.";
                }
            }
            catch (Exception ex)
            {
                respuesta.Estado = false;
                respuesta.Mensaje = $"Error: {ex.Message}";
            }

            return respuesta;
        }

        public Respuesta<List<EResultadoGeneral>> ResultadoGeneralVotacion(int IdEleccion)
        {
            try
            {
                List<EResultadoGeneral> rptLista = new List<EResultadoGeneral>();

                using (SqlConnection con = ConexionBD.GetInstance().ConexionDB())
                {
                    using (SqlCommand comando = new SqlCommand("usp_ResultadoGeneralVotacion", con))
                    {
                        comando.CommandType = CommandType.StoredProcedure;
                        comando.Parameters.AddWithValue("@IdEleccion", IdEleccion);
                        con.Open();

                        using (SqlDataReader dr = comando.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                rptLista.Add(new EResultadoGeneral()
                                {
                                    NombrePartido = dr["NombrePartido"].ToString(),
                                    Sigla = dr["Sigla"].ToString(),
                                    TotalVotos = Convert.ToInt32(dr["TotalVotos"])
                                });
                            }
                        }
                    }
                }
                return new Respuesta<List<EResultadoGeneral>>()
                {
                    Estado = true,
                    Data = rptLista,
                    Mensaje = "Datos obtenidos correctamente"
                };
            }
            catch (Exception ex)
            {
                // Maneja cualquier error inesperado
                return new Respuesta<List<EResultadoGeneral>>()
                {
                    Estado = false,
                    Mensaje = "Ocurrió un error: " + ex.Message,
                    Data = null
                };
            }
        }

    }
}
