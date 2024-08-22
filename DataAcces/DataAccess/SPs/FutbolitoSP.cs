using System;
using System.Data;
using System.Data.SqlClient;

namespace DataAcces.DataAccess.SPs
{
    public class FutbolitoSP
    {
        private readonly DatabaseHelper _dbHelper;

        public FutbolitoSP(DatabaseHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        public bool CrearPartida(long identificadorUsuario, out long identificadorJuego, out string descripcionError)
        {
            identificadorJuego = 0;
            descripcionError = string.Empty;

            try
            {
                using (var connection = _dbHelper.GetConnection())
                {
                    using (var command = new SqlCommand("SP_FutbolitoCrearPartida", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@V_identificadorUsuario1", identificadorUsuario);

                        var juegoParam = new SqlParameter("@V_IdentificadorJuego", SqlDbType.BigInt)
                        {
                            Direction = ParameterDirection.Output
                        };
                        command.Parameters.Add(juegoParam);

                        var errorParam = new SqlParameter("@V_DescripcionError", SqlDbType.NVarChar, 255)
                        {
                            Direction = ParameterDirection.Output
                        };
                        command.Parameters.Add(errorParam);

                        connection.Open();
                        command.ExecuteNonQuery();

                        identificadorJuego = juegoParam.Value != DBNull.Value ? (long)juegoParam.Value : 0;
                        descripcionError = errorParam.Value.ToString();

                        return string.IsNullOrEmpty(descripcionError);
                    }
                }
            }
            catch (Exception ex)
            {
                descripcionError = "Error al crear la partida: " + ex.Message;
                return false;
            }
        }

        public bool BuscarPartida(long identificadorUsuario, out long identificadorJuego, out int identificadorTurno, out long identificadorUsuario1, out string nombreUsuario1, out string descripcionError)
        {
            identificadorJuego = 0;
            identificadorTurno = 0;
            identificadorUsuario1 = 0;
            nombreUsuario1 = string.Empty;
            descripcionError = string.Empty;

            try
            {
                using (var connection = _dbHelper.GetConnection())
                {
                    using (var command = new SqlCommand("SP_FutbolitoBuscarPartida", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Parámetros de entrada
                        command.Parameters.AddWithValue("@V_IdentificadorUsuario", identificadorUsuario);

                        // Parámetros de salida
                        command.Parameters.Add("@V_IdentificadorJuego", SqlDbType.BigInt).Direction = ParameterDirection.Output;
                        command.Parameters.Add("@V_IdentificadorTurno", SqlDbType.Int).Direction = ParameterDirection.Output;
                        command.Parameters.Add("@V_IdentificadorUsuario1", SqlDbType.BigInt).Direction = ParameterDirection.Output;
                        command.Parameters.Add("@V_nombreUsuario1", SqlDbType.NVarChar, 255).Direction = ParameterDirection.Output;
                        command.Parameters.Add("@V_DescripcionError", SqlDbType.NVarChar, 255).Direction = ParameterDirection.Output;

                        connection.Open();
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                identificadorJuego = reader.GetInt64(reader.GetOrdinal("identificadorJuego"));

                                // Aquí aplicas la verificación de DBNull
                                identificadorTurno = reader.IsDBNull(reader.GetOrdinal("identificadorTurno"))
                                    ? 0
                                    : reader.GetInt32(reader.GetOrdinal("identificadorTurno"));

                                identificadorUsuario1 = reader.GetInt64(reader.GetOrdinal("identificadorUsuario1"));
                                nombreUsuario1 = reader.GetString(reader.GetOrdinal("nombreUsuario1"));
                            }
                        }

                        descripcionError = command.Parameters["@V_DescripcionError"].Value.ToString();
                    }
                }

                return string.IsNullOrEmpty(descripcionError);
            }
            catch (Exception ex)
            {
                descripcionError = "Error al buscar la partida: " + ex.Message;
                return false;
            }
        }


        public int ObtenerPuntuacionFutbolitoUsuario(long identificadorUsuario)
        {
            int puntajeUsuario = 0;

            try
            {
                using (var connection = _dbHelper.GetConnection())
                {
                    using (var command = new SqlCommand("SP_ObtenerPuntuacionFutbolitoUsuario", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Añadir los parámetros
                        command.Parameters.AddWithValue("@V_IdentificadorUsuario", identificadorUsuario);
                        SqlParameter puntajeParam = new SqlParameter("@V_PuntajeUsuario", SqlDbType.Int);
                        puntajeParam.Direction = ParameterDirection.Output;
                        command.Parameters.Add(puntajeParam);

                        connection.Open();
                        command.ExecuteNonQuery();

                        // Obtener el valor del puntaje
                        puntajeUsuario = (int)puntajeParam.Value;
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejo de excepciones
                throw new Exception("Error al obtener la puntuación del usuario en Futbolito: " + ex.Message);
            }

            return puntajeUsuario;
        }
    }
}

