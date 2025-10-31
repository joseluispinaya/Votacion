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
    public class DDelegado
    {
        #region "PATRON SINGLETON"
        public static DDelegado _instancia = null;

        private DDelegado()
        {

        }

        public static DDelegado GetInstance()
        {
            if (_instancia == null)
            {
                _instancia = new DDelegado();
            }
            return _instancia;
        }
        #endregion

        public Respuesta<bool> RegistrarDelegadosNuevo(EDelegado oDelegado)
        {
            try
            {
                using (SqlConnection con = ConexionBD.GetInstance().ConexionDB())
                {
                    using (SqlCommand cmd = new SqlCommand("usp_RegistrarDelegadoNuevo", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Parámetros de entrada
                        cmd.Parameters.AddWithValue("@IdPersona", oDelegado.IdPersona);
                        cmd.Parameters.AddWithValue("@IdEleccion", oDelegado.IdEleccion);
                        cmd.Parameters.AddWithValue("@IdMesa", oDelegado.IdMesa);

                        // Parámetro de salida
                        SqlParameter outputParam = new SqlParameter("@Resultado", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(outputParam);

                        // Ejecutar
                        con.Open();
                        cmd.ExecuteNonQuery();

                        // Leer resultado
                        int resultado = Convert.ToInt32(outputParam.Value);

                        // Evaluar resultado
                        switch (resultado)
                        {
                            case 1:
                                return new Respuesta<bool>
                                {
                                    Estado = true,
                                    Mensaje = "Delegado asignado correctamente a la mesa.",
                                    Data = true
                                };

                            case 2:
                                return new Respuesta<bool>
                                {
                                    Estado = false,
                                    Mensaje = "La mesa ya tiene un delegado asignado.",
                                    Data = false
                                };

                            case 3:
                                return new Respuesta<bool>
                                {
                                    Estado = false,
                                    Mensaje = "Este delegado ya tiene mesas asignadas en OTRO RECINTO.",
                                    Data = false
                                };

                            case 0:
                            default:
                                return new Respuesta<bool>
                                {
                                    Estado = false,
                                    Mensaje = "Error al registrar el delegado (Error interno BD).",
                                    Data = false
                                };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return new Respuesta<bool>
                {
                    Estado = false,
                    Mensaje = "Ocurrió un error inesperado: " + ex.Message,
                    Data = false
                };
            }
        }

        public Respuesta<bool> RegistrarDelegados(EDelegado oDelegado)
        {
            try
            {
                using (SqlConnection con = ConexionBD.GetInstance().ConexionDB())
                {
                    using (SqlCommand cmd = new SqlCommand("usp_RegistrarDelegado", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Parámetros de entrada
                        cmd.Parameters.AddWithValue("@IdPersona", oDelegado.IdPersona);
                        cmd.Parameters.AddWithValue("@IdEleccion", oDelegado.IdEleccion);
                        cmd.Parameters.AddWithValue("@IdMesa", oDelegado.IdMesa);

                        // Parámetro de salida
                        SqlParameter outputParam = new SqlParameter("@Resultado", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(outputParam);

                        // Ejecutar
                        con.Open();
                        cmd.ExecuteNonQuery();

                        // Leer resultado
                        int resultado = Convert.ToInt32(outputParam.Value);

                        // Evaluar resultado
                        switch (resultado)
                        {
                            case 1:
                                return new Respuesta<bool>
                                {
                                    Estado = true,
                                    Mensaje = "Delegado registrado correctamente.",
                                    Data = true
                                };

                            case 2:
                                return new Respuesta<bool>
                                {
                                    Estado = false,
                                    Mensaje = "Ya existe un Delegado asignado a la mesa.",
                                    Data = false
                                };

                            case 0:
                            default:
                                return new Respuesta<bool>
                                {
                                    Estado = false,
                                    Mensaje = "Error al registrar el Delegado (error en BD).",
                                    Data = false
                                };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return new Respuesta<bool>
                {
                    Estado = false,
                    Mensaje = "Ocurrió un error: " + ex.Message,
                    Data = false
                };
            }
        }

        public Respuesta<List<EMesa>> ListaMesasSelect(int IdRecinto, int IdEleccion)
        {
            try
            {
                List<EMesa> rptLista = new List<EMesa>();

                using (SqlConnection con = ConexionBD.GetInstance().ConexionDB())
                {
                    using (SqlCommand comando = new SqlCommand("usp_MesasIdRecintoEleccionNu", con))
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

        public Respuesta<List<EMesaDto>> ListaMesasDelegados(int IdRecinto, int IdEleccion)
        {
            try
            {
                List<EMesaDto> rptLista = new List<EMesaDto>();

                using (SqlConnection con = ConexionBD.GetInstance().ConexionDB())
                {
                    using (SqlCommand comando = new SqlCommand("usp_ListarMesasConDelegado", con))
                    {
                        comando.CommandType = CommandType.StoredProcedure;
                        comando.Parameters.AddWithValue("@IdRecinto", IdRecinto);
                        comando.Parameters.AddWithValue("@IdEleccion", IdEleccion);
                        con.Open();

                        using (SqlDataReader dr = comando.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                rptLista.Add(new EMesaDto()
                                {
                                    IdMesa = Convert.ToInt32(dr["IdMesa"]),
                                    NumeroMesa = Convert.ToInt32(dr["NumeroMesa"]),
                                    NombreCompleto = dr["NombreDelegado"].ToString(),
                                    Celular = dr["Celular"].ToString()
                                });
                            }
                        }
                    }
                }
                return new Respuesta<List<EMesaDto>>()
                {
                    Estado = true,
                    Data = rptLista,
                    Mensaje = "Mesa obtenidos correctamente"
                };
            }
            catch (Exception ex)
            {
                // Maneja cualquier error inesperado
                return new Respuesta<List<EMesaDto>>()
                {
                    Estado = false,
                    Mensaje = "Ocurrió un error: " + ex.Message,
                    Data = null
                };
            }
        }

        public Respuesta<List<EListaMesasDto>> MesasAsignadasDelegados(int IdPersona, int IdEleccion)
        {
            try
            {
                List<EListaMesasDto> rptLista = new List<EListaMesasDto>();

                using (SqlConnection con = ConexionBD.GetInstance().ConexionDB())
                {
                    using (SqlCommand comando = new SqlCommand("ObtenerMesasPorDelegado", con))
                    {
                        comando.CommandType = CommandType.StoredProcedure;
                        comando.Parameters.AddWithValue("@IdPersona", IdPersona);
                        comando.Parameters.AddWithValue("@IdEleccion", IdEleccion);
                        con.Open();

                        using (SqlDataReader dr = comando.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                rptLista.Add(new EListaMesasDto()
                                {
                                    IdDelegado = Convert.ToInt32(dr["IdDelegado"]),
                                    IdEleccion = Convert.ToInt32(dr["IdEleccion"]),
                                    IdMesa = Convert.ToInt32(dr["IdMesa"]),
                                    NumeroMesa = Convert.ToInt32(dr["NumeroMesa"]),
                                    NombreRecinto = dr["NombreRecinto"].ToString(),
                                    NombreLocalidad = dr["NombreLocalidad"].ToString(),
                                    NombreEleccion = dr["NombreEleccion"].ToString()
                                });
                            }
                        }
                    }
                }
                return new Respuesta<List<EListaMesasDto>>()
                {
                    Estado = true,
                    Data = rptLista,
                    Mensaje = "Mesa obtenidos correctamente"
                };
            }
            catch (Exception ex)
            {
                // Maneja cualquier error inesperado
                return new Respuesta<List<EListaMesasDto>>()
                {
                    Estado = false,
                    Mensaje = "Ocurrió un error: " + ex.Message,
                    Data = null
                };
            }
        }

        public Respuesta<EDelegado> LoginDelegado(string correo, string claveHash)
        {
            try
            {
                EDelegado obj = null;

                using (SqlConnection con = ConexionBD.GetInstance().ConexionDB())
                {
                    using (SqlCommand comando = new SqlCommand("usp_LoginDelegado", con))
                    {
                        comando.CommandType = CommandType.StoredProcedure;
                        comando.Parameters.AddWithValue("@Correo", correo);
                        comando.Parameters.AddWithValue("@ClaveHash", claveHash);

                        con.Open();
                        using (SqlDataReader dr = comando.ExecuteReader())
                        {
                            if (dr.Read())
                            {
                                obj = new EDelegado
                                {
                                    IdDelegado = Convert.ToInt32(dr["IdDelegado"]),
                                    IdPersona = Convert.ToInt32(dr["IdPersona"]),
                                    IdEleccion = Convert.ToInt32(dr["IdEleccion"]),
                                    RefPersona = new EPersona
                                    {
                                        NombreCompleto = dr["NombreCompleto"].ToString(),
                                        Correo = dr["Correo"].ToString(),
                                        CI = dr["CI"].ToString(),
                                        Celular = dr["Celular"].ToString(),
                                        Estado = Convert.ToBoolean(dr["Estado"])
                                    }
                                    // demas codigo
                                };
                            }
                        }
                    }
                }

                return new Respuesta<EDelegado>
                {
                    Estado = obj != null,
                    Data = obj,
                    Mensaje = obj != null ? "Delegado obtenido correctamente" : "Credenciales incorrectas o delegado no encontrado"
                };
            }
            catch (Exception ex)
            {
                // Manejo de excepciones generales
                return new Respuesta<EDelegado>
                {
                    Estado = false,
                    Mensaje = "Ocurrió un error inesperado: " + ex.Message,
                    Data = null
                };
            }
        }

        public Respuesta<EAdministrador> LoginAdmin(string correo, string claveHash)
        {
            try
            {
                EAdministrador obj = null;

                using (SqlConnection con = ConexionBD.GetInstance().ConexionDB())
                {
                    using (SqlCommand comando = new SqlCommand("usp_LoginAdmin", con))
                    {
                        comando.CommandType = CommandType.StoredProcedure;
                        comando.Parameters.AddWithValue("@Correo", correo);
                        comando.Parameters.AddWithValue("@ClaveHash", claveHash);

                        con.Open();
                        using (SqlDataReader dr = comando.ExecuteReader())
                        {
                            if (dr.Read())
                            {
                                obj = new EAdministrador
                                {
                                    IdAdmin = Convert.ToInt32(dr["IdAdmin"]),
                                    Nombres = dr["Nombres"].ToString(),
                                    Apellidos = dr["Apellidos"].ToString(),
                                    Correo = dr["Correo"].ToString(),
                                    Clave = dr["Clave"].ToString(),
                                    Estado = Convert.ToBoolean(dr["Estado"])
                                };
                            }
                        }
                    }
                }

                return new Respuesta<EAdministrador>
                {
                    Estado = obj != null,
                    Data = obj,
                    Mensaje = obj != null ? "Administrador obtenido correctamente" : "Credenciales incorrectas o Administrador no encontrado"
                };
            }
            catch (Exception ex)
            {
                // Manejo de excepciones generales
                return new Respuesta<EAdministrador>
                {
                    Estado = false,
                    Mensaje = "Ocurrió un error inesperado: " + ex.Message,
                    Data = null
                };
            }
        }

        public Respuesta<List<EPartidoPol>> ListaPartidosVota(int IdEleccion, int IdMesa)
        {
            try
            {
                List<EPartidoPol> rptLista = new List<EPartidoPol>();

                using (SqlConnection con = ConexionBD.GetInstance().ConexionDB())
                {
                    using (SqlCommand comando = new SqlCommand("ListarPartidosPorMesa", con))
                    {
                        comando.CommandType = CommandType.StoredProcedure;
                        comando.Parameters.AddWithValue("@IdEleccion", IdEleccion);
                        comando.Parameters.AddWithValue("@IdMesa", IdMesa);
                        con.Open();

                        using (SqlDataReader dr = comando.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                rptLista.Add(new EPartidoPol()
                                {
                                    IdPartido = Convert.ToInt32(dr["IdPartido"]),
                                    Nombre = dr["Nombre"].ToString(),
                                    Sigla = dr["Sigla"].ToString()
                                });
                            }
                        }
                    }
                }
                return new Respuesta<List<EPartidoPol>>()
                {
                    Estado = true,
                    Data = rptLista,
                    Mensaje = "PartidoPol obtenidos correctamente"
                };
            }
            catch (Exception ex)
            {
                // Maneja cualquier error inesperado
                return new Respuesta<List<EPartidoPol>>()
                {
                    Estado = false,
                    Mensaje = "Ocurrió un error: " + ex.Message,
                    Data = null
                };
            }
        }

    }
}
