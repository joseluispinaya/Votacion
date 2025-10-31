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

                    // Guardar los votos por partido (ListaResultados)
                    foreach (var item in ListaResultados)
                    {
                        using (SqlCommand cmd = new SqlCommand("RegistrarResultado", con))
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
                        }
                    }

                    // Guardar votos especiales (Nulos / Blancos)
                    using (SqlCommand cmd = new SqlCommand("RegistrarVotoEspecial", con))
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
                    }

                    respuesta.Estado = true;
                    respuesta.Mensaje = "Se registró correctamente.";
                    respuesta.Data = true;
                }
            }
            catch (Exception ex)
            {
                respuesta.Estado = false;
                respuesta.Mensaje = $"Ocurrió un error: {ex.Message}";
                respuesta.Data = false;
            }

            return respuesta;
        }

    }
}
