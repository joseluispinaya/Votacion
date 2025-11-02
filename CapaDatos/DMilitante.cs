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
    public class DMilitante
    {
        #region "PATRON SINGLETON"
        public static DMilitante _instancia = null;

        private DMilitante()
        {

        }

        public static DMilitante GetInstance()
        {
            if (_instancia == null)
            {
                _instancia = new DMilitante();
            }
            return _instancia;
        }
        #endregion

        public Respuesta<List<EDistrito>> ListaDistritos()
        {
            try
            {
                List<EDistrito> rptLista = new List<EDistrito>();

                using (SqlConnection con = ConexionBD.GetInstance().ConexionDB())
                {
                    using (SqlCommand comando = new SqlCommand("usp_ObtenerDistritos", con))
                    {
                        comando.CommandType = CommandType.StoredProcedure;
                        con.Open();

                        using (SqlDataReader dr = comando.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                rptLista.Add(new EDistrito()
                                {
                                    IdDistrito = Convert.ToInt32(dr["IdDistrito"]),
                                    Distrito = dr["Distrito"].ToString()
                                });
                            }
                        }
                    }
                }
                return new Respuesta<List<EDistrito>>()
                {
                    Estado = true,
                    Data = rptLista,
                    Mensaje = "Distritos obtenidos correctamente"
                };
            }
            catch (Exception ex)
            {
                // Maneja cualquier error inesperado
                return new Respuesta<List<EDistrito>>()
                {
                    Estado = false,
                    Mensaje = "Ocurrió un error: " + ex.Message,
                    Data = null
                };
            }
        }

        public Respuesta<bool> RegistrarMilitante(EMilitante oPersona)
        {
            try
            {
                bool respuesta = false;
                using (SqlConnection con = ConexionBD.GetInstance().ConexionDB())
                {
                    using (SqlCommand cmd = new SqlCommand("usp_RegistrarMilitantes", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@NombresCompleto", oPersona.NombresCompleto);
                        cmd.Parameters.AddWithValue("@NroCI", oPersona.NroCI);
                        cmd.Parameters.AddWithValue("@FechaNacido", oPersona.VFechaNacido);
                        cmd.Parameters.AddWithValue("@Celular", oPersona.Celular);
                        cmd.Parameters.AddWithValue("@IdDistrito", oPersona.IdDistrito);

                        SqlParameter outputParam = new SqlParameter("@Resultado", SqlDbType.Bit)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(outputParam);

                        con.Open();
                        cmd.ExecuteNonQuery();
                        respuesta = Convert.ToBoolean(outputParam.Value);
                    }
                }
                return new Respuesta<bool>
                {
                    Estado = respuesta,
                    Mensaje = respuesta ? "Se registro correctamente" : "Error al registrar intente mas tarde"
                };
            }
            catch (Exception ex)
            {
                return new Respuesta<bool> { Estado = false, Mensaje = "Ocurrió un error: " + ex.Message };
            }
        }

        public Respuesta<bool> EditarMilitante(EMilitante oPersona)
        {
            try
            {
                bool respuesta = false;
                using (SqlConnection con = ConexionBD.GetInstance().ConexionDB())
                {
                    using (SqlCommand cmd = new SqlCommand("usp_ModificarMilitantes", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@IdMilitante", oPersona.IdMilitante);
                        cmd.Parameters.AddWithValue("@NombresCompleto", oPersona.NombresCompleto);
                        cmd.Parameters.AddWithValue("@NroCI", oPersona.NroCI);
                        cmd.Parameters.AddWithValue("@FechaNacido", oPersona.VFechaNacido);
                        cmd.Parameters.AddWithValue("@Celular", oPersona.Celular);
                        cmd.Parameters.AddWithValue("@IdDistrito", oPersona.IdDistrito);
                        cmd.Parameters.AddWithValue("@Estado", oPersona.Estado);

                        SqlParameter outputParam = new SqlParameter("@Resultado", SqlDbType.Bit)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(outputParam);

                        con.Open();
                        cmd.ExecuteNonQuery();
                        respuesta = Convert.ToBoolean(outputParam.Value);
                    }
                }
                return new Respuesta<bool>
                {
                    Estado = respuesta,
                    Mensaje = respuesta ? "Se actualizo correctamente" : "Error al actualizar intente mas tarde"
                };
            }
            catch (Exception ex)
            {
                return new Respuesta<bool> { Estado = false, Mensaje = "Ocurrió un error: " + ex.Message };
            }
        }

        public Respuesta<List<EMilitante>> ListaMilitantes()
        {
            try
            {
                List<EMilitante> rptLista = new List<EMilitante>();

                using (SqlConnection con = ConexionBD.GetInstance().ConexionDB())
                {
                    using (SqlCommand comando = new SqlCommand("usp_ObtenerMilitantes", con))
                    {
                        comando.CommandType = CommandType.StoredProcedure;
                        con.Open();

                        using (SqlDataReader dr = comando.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                rptLista.Add(new EMilitante()
                                {
                                    IdMilitante = Convert.ToInt32(dr["IdMilitante"]),
                                    NombresCompleto = dr["NombresCompleto"].ToString(),
                                    NroCI = dr["NroCI"].ToString(),
                                    FechaNacido = Convert.ToDateTime(dr["FechaNacido"]).ToString("dd/MM/yyyy"),
                                    VFechaNacido = Convert.ToDateTime(dr["FechaNacido"].ToString()),
                                    Celular = dr["Celular"].ToString(),
                                    IdDistrito = Convert.ToInt32(dr["IdDistrito"]),
                                    Estado = Convert.ToBoolean(dr["Estado"]),
                                    FechaRegistro = Convert.ToDateTime(dr["FechaRegistro"]).ToString("dd/MM/yyyy"),
                                    VFechaRegistro = Convert.ToDateTime(dr["FechaRegistro"].ToString()),
                                    RefDistrito = new EDistrito
                                    {
                                        Distrito = dr["Distrito"].ToString()
                                    }
                                });
                            }
                        }
                    }
                }
                return new Respuesta<List<EMilitante>>()
                {
                    Estado = true,
                    Data = rptLista,
                    Mensaje = "EMilitante obtenidos correctamente"
                };
            }
            catch (Exception ex)
            {
                // Maneja cualquier error inesperado
                return new Respuesta<List<EMilitante>>()
                {
                    Estado = false,
                    Mensaje = "Ocurrió un error: " + ex.Message,
                    Data = null
                };
            }
        }

    }
}
