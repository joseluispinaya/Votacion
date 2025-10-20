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
    public class DLocalidad
    {
        #region "PATRON SINGLETON"
        public static DLocalidad _instancia = null;

        private DLocalidad()
        {

        }

        public static DLocalidad GetInstance()
        {
            if (_instancia == null)
            {
                _instancia = new DLocalidad();
            }
            return _instancia;
        }
        #endregion

        public Respuesta<List<ELocalidad>> ListaLocalidades()
        {
            try
            {
                List<ELocalidad> rptLista = new List<ELocalidad>();

                using (SqlConnection con = ConexionBD.GetInstance().ConexionDB())
                {
                    using (SqlCommand comando = new SqlCommand("usp_ObtenerLocalidades", con))
                    {
                        comando.CommandType = CommandType.StoredProcedure;
                        con.Open();

                        using (SqlDataReader dr = comando.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                rptLista.Add(new ELocalidad()
                                {
                                    IdLocalidad = Convert.ToInt32(dr["IdLocalidad"]),
                                    Nombre = dr["Nombre"].ToString()
                                });
                            }
                        }
                    }
                }
                return new Respuesta<List<ELocalidad>>()
                {
                    Estado = true,
                    Data = rptLista,
                    Mensaje = "Localidad obtenidos correctamente"
                };
            }
            catch (Exception ex)
            {
                // Maneja cualquier error inesperado
                return new Respuesta<List<ELocalidad>>()
                {
                    Estado = false,
                    Mensaje = "Ocurrió un error: " + ex.Message,
                    Data = null
                };
            }
        }

        public Respuesta<List<ERecinto>> ListaRecintos(int IdLocalidad)
        {
            try
            {
                List<ERecinto> rptLista = new List<ERecinto>();

                using (SqlConnection con = ConexionBD.GetInstance().ConexionDB())
                {
                    using (SqlCommand comando = new SqlCommand("usp_RecintosIdLocali", con))
                    {
                        comando.CommandType = CommandType.StoredProcedure;
                        comando.Parameters.AddWithValue("@IdLocalidad", IdLocalidad);
                        con.Open();

                        using (SqlDataReader dr = comando.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                rptLista.Add(new ERecinto()
                                {
                                    IdRecinto = Convert.ToInt32(dr["IdRecinto"]),
                                    IdLocalidad = Convert.ToInt32(dr["IdLocalidad"]),
                                    Nombre = dr["Nombre"].ToString(),
                                    Estado = Convert.ToBoolean(dr["Estado"])
                                });
                            }
                        }
                    }
                }
                return new Respuesta<List<ERecinto>>()
                {
                    Estado = true,
                    Data = rptLista,
                    Mensaje = "Recintos obtenidos correctamente"
                };
            }
            catch (Exception ex)
            {
                // Maneja cualquier error inesperado
                return new Respuesta<List<ERecinto>>()
                {
                    Estado = false,
                    Mensaje = "Ocurrió un error: " + ex.Message,
                    Data = null
                };
            }
        }

        public Respuesta<List<EMesa>> ListaMesas(int IdRecinto)
        {
            try
            {
                List<EMesa> rptLista = new List<EMesa>();

                using (SqlConnection con = ConexionBD.GetInstance().ConexionDB())
                {
                    using (SqlCommand comando = new SqlCommand("usp_MesasIdRecinto", con))
                    {
                        comando.CommandType = CommandType.StoredProcedure;
                        comando.Parameters.AddWithValue("@IdRecinto", IdRecinto);
                        con.Open();

                        using (SqlDataReader dr = comando.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                rptLista.Add(new EMesa()
                                {
                                    IdMesa = Convert.ToInt32(dr["IdMesa"]),
                                    IdRecinto = Convert.ToInt32(dr["IdRecinto"]),
                                    NumeroMesa = Convert.ToInt32(dr["NumeroMesa"]),
                                    Estado = Convert.ToBoolean(dr["Estado"])
                                });
                            }
                        }
                    }
                }
                return new Respuesta<List<EMesa>>()
                {
                    Estado = true,
                    Data = rptLista,
                    Mensaje = "Mesa obtenidos correctamente"
                };
            }
            catch (Exception ex)
            {
                // Maneja cualquier error inesperado
                return new Respuesta<List<EMesa>>()
                {
                    Estado = false,
                    Mensaje = "Ocurrió un error: " + ex.Message,
                    Data = null
                };
            }
        }

    }
}
