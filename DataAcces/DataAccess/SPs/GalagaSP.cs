using DataAcces.Entities;
using System;
using System.Data;
using System.Data.SqlClient;

namespace DataAcces.DataAcces
{
    public class GalagaSP
    {
        private readonly DatabaseHelper _dbHelper;

        public GalagaSP(DatabaseHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        public bool CrearPartida(long identificadorUsuario, int puntajeUsuario, float duracion, out long identificadorJuego, out string descripcionError)
        {
            identificadorJuego = 0;
            descripcionError = null;
            bool resultado = false;

            try
            {
                using (var connection = _dbHelper.GetConnection())
                {
                    using (var command = new SqlCommand("SP_GalagaCrearPartida", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Añadir parámetros
                        command.Parameters.AddWithValue("@V_IdentificadorUsuario", identificadorUsuario);
                        command.Parameters.AddWithValue("@V_PuntajeUsuario", puntajeUsuario);
                        command.Parameters.AddWithValue("@V_Duracion", duracion);

                        SqlParameter identificadorJuegoParam = new SqlParameter("@V_IdentificadorJuego", SqlDbType.BigInt)
                        {
                            Direction = ParameterDirection.Output
                        };
                        command.Parameters.Add(identificadorJuegoParam);

                        SqlParameter descripcionErrorParam = new SqlParameter("@V_DescripcionError", SqlDbType.NVarChar, 255)
                        {
                            Direction = ParameterDirection.Output
                        };
                        command.Parameters.Add(descripcionErrorParam);

                        connection.Open();

                        // Ejecutar el procedimiento almacenado
                        command.ExecuteNonQuery();

                        // Obtener el mensaje de error y el identificador del juego
                        identificadorJuego = (long)identificadorJuegoParam.Value;
                        descripcionError = descripcionErrorParam.Value.ToString();

                        if (string.IsNullOrEmpty(descripcionError))
                        {
                            resultado = true;  // Partida creada correctamente
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                descripcionError = "Error al crear la partida en la base de datos: " + ex.Message;
            }

            return resultado;
        }

        public int ObtenerPuntuacionUsuario(long usuarioId)
        {
            int resultado = 0;

            try
            {
                using (var connection = _dbHelper.GetConnection())
                {
                    using (var command = new SqlCommand("SP_GalagaPuntuacionUsuario", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Añadir parámetros
                        command.Parameters.AddWithValue("@V_Usuario", usuarioId);

                        connection.Open();

                        // Ejecutar el procedimiento almacenado y obtener el resultado
                        var result = command.ExecuteScalar();
                        if (result != null)
                        {
                            resultado = Convert.ToInt32(result);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejar la excepción según sea necesario
                throw new Exception("Error al obtener la puntuación del usuario: " + ex.Message);
            }

            return resultado;
        }

        public List<PartidaGalaga> ObtenerPuntuacionesUsuarios()
        {
            List<PartidaGalaga> puntuaciones = new List<PartidaGalaga>();

            try
            {
                using (var connection = _dbHelper.GetConnection())
                {
                    using (var command = new SqlCommand("SP_GalagaObtenerPuntuacionesUsuarios", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        connection.Open();
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var puntuacion = new PartidaGalaga
                                {
                                    indentificadorJuegoGalaga = reader.GetInt64(0),
                                    identificadorUsuario = reader.GetInt64(0),
                                    puntajeUsuario = reader.GetInt32(1),
                                    duracion = reader.GetFloat(2)
                                };
                                puntuaciones.Add(puntuacion);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejo de errores
                throw new Exception("Error al obtener las puntuaciones de los usuarios: " + ex.Message);
            }

            return puntuaciones;
        }
    }
}
