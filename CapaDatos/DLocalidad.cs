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

        public Respuesta<List<EMesa>> ListaMesas(int IdRecinto, int IdEleccion)
        {
            try
            {
                List<EMesa> rptLista = new List<EMesa>();

                using (SqlConnection con = ConexionBD.GetInstance().ConexionDB())
                {
                    using (SqlCommand comando = new SqlCommand("usp_MesasIdRecintoEleccion", con))
                    {
                        comando.CommandType = CommandType.StoredProcedure;
                        comando.Parameters.AddWithValue("@IdRecinto", IdRecinto);
                        comando.Parameters.AddWithValue("@IdEleccion", IdEleccion);
                        con.Open();

                        using (SqlDataReader dr = comando.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                rptLista.Add(new EMesa()
                                {
                                    IdMesa = Convert.ToInt32(dr["IdMesa"]),
                                    IdRecinto = Convert.ToInt32(dr["IdRecinto"]),
                                    IdEleccion = Convert.ToInt32(dr["IdEleccion"]),
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

        //elecciones
        public Respuesta<List<ETipoEleccion>> ListaTiposEle()
        {
            try
            {
                List<ETipoEleccion> rptLista = new List<ETipoEleccion>();

                using (SqlConnection con = ConexionBD.GetInstance().ConexionDB())
                {
                    using (SqlCommand comando = new SqlCommand("usp_ObtenerTipoEleccion", con))
                    {
                        comando.CommandType = CommandType.StoredProcedure;
                        con.Open();

                        using (SqlDataReader dr = comando.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                rptLista.Add(new ETipoEleccion()
                                {
                                    IdTipo = Convert.ToInt32(dr["IdTipo"]),
                                    Nombre = dr["Nombre"].ToString()
                                });
                            }
                        }
                    }
                }
                return new Respuesta<List<ETipoEleccion>>()
                {
                    Estado = true,
                    Data = rptLista,
                    Mensaje = "TipoEleccion obtenidos correctamente"
                };
            }
            catch (Exception ex)
            {
                // Maneja cualquier error inesperado
                return new Respuesta<List<ETipoEleccion>>()
                {
                    Estado = false,
                    Mensaje = "Ocurrió un error: " + ex.Message,
                    Data = null
                };
            }
        }

        public Respuesta<List<EEleccion>> ListaElecciones()
        {
            try
            {
                List<EEleccion> rptLista = new List<EEleccion>();

                using (SqlConnection con = ConexionBD.GetInstance().ConexionDB())
                {
                    using (SqlCommand comando = new SqlCommand("ObtenerElecciones", con))
                    {
                        comando.CommandType = CommandType.StoredProcedure;
                        con.Open();

                        using (SqlDataReader dr = comando.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                rptLista.Add(new EEleccion()
                                {
                                    IdEleccion = Convert.ToInt32(dr["IdEleccion"]),
                                    Descripcion = dr["Descripcion"].ToString(),
                                    Estado = Convert.ToBoolean(dr["Estado"]),
                                    FechaRegistro = Convert.ToDateTime(dr["FechaRegistro"]).ToString("dd/MM/yyyy"),
                                    RefTipoEleccion = new ETipoEleccion
                                    {
                                        Nombre = dr["Tipo"].ToString()
                                    }
                                });
                            }
                        }
                    }
                }
                return new Respuesta<List<EEleccion>>()
                {
                    Estado = true,
                    Data = rptLista,
                    Mensaje = "EEleccion obtenidos correctamente"
                };
            }
            catch (Exception ex)
            {
                // Maneja cualquier error inesperado
                return new Respuesta<List<EEleccion>>()
                {
                    Estado = false,
                    Mensaje = "Ocurrió un error: " + ex.Message,
                    Data = null
                };
            }
        }

        public Respuesta<bool> RegistrarEleccion(EEleccion oEleccion)
        {
            try
            {
                bool respuesta = false;
                using (SqlConnection con = ConexionBD.GetInstance().ConexionDB())
                {
                    using (SqlCommand cmd = new SqlCommand("usp_RegistrarEleccion", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@IdTipo", oEleccion.IdTipo);
                        cmd.Parameters.AddWithValue("@IdAdmin", oEleccion.IdAdmin);
                        cmd.Parameters.AddWithValue("@Descripcion", oEleccion.Descripcion);

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

    }
}
