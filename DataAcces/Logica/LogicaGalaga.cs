using DataAcces.DataAcces;
using DataAcces.Entities;
using DataAccess.SPs;
using System.Data.SqlClient;
using System.Data;


namespace DataAcces.Logica
{
    public class LogicaGalaga
    {
        private readonly GalagaSP _galagaSP;
        private readonly DatabaseHelper _dbHelper;

        // Constructor que acepta ambos parámetros
        public LogicaGalaga(GalagaSP galagaSP, DatabaseHelper dbHelper)
        {
            _galagaSP = galagaSP;
            _dbHelper = dbHelper;
        }

        public ResGalagaPartidaInsertar CrearPartida(ReqGalagaPartidaInsertar req)
        {
            ResGalagaPartidaInsertar res = new ResGalagaPartidaInsertar();

            if (req == null || req.user == null)
            {
                res.resultado = false;
                res.descripcionError = "Solicitud inválida";
                return res;
            }

            // Llamada al procedimiento almacenado para crear la partida
            bool resultado = _galagaSP.CrearPartida(
                req.user.identificadorUsuario,
                req.user.puntajeUsuario,
                req.user.duracion,
                out long identificadorJuego,
                out string descripcionError
            );

            // Verificación del resultado
            if (resultado)
            {
                res.resultado = true;
                res.identificadorJuego = identificadorJuego;
            }
            else
            {
                res.resultado = false;
                res.descripcionError = "No se pudo crear la partida: " + descripcionError;
            }

            return res;
        }
        public List<PartidaGalaga> ObtenerPuntuacionUsuario(long usuarioId)
        {
            List<PartidaGalaga> puntuaciones = new List<PartidaGalaga>();

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

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                PartidaGalaga puntuacion = new PartidaGalaga
                                {
                                    indentificadorJuegoGalaga = reader.GetInt64(reader.GetOrdinal("identificadorJuego")),
                                    identificadorUsuario = reader.GetInt64(reader.GetOrdinal("identificadorUsuario1")),
                                    puntajeUsuario = reader.GetInt32(reader.GetOrdinal("PuntajeUsuario1")),
                                    duracion = reader.GetDouble(reader.GetOrdinal("duracion")),
                                };
                                puntuaciones.Add(puntuacion);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejar la excepción según sea necesario
                throw new Exception("Error al obtener la puntuación del usuario: " + ex.Message);
            }

            return puntuaciones;
        }


        public List<PartidaGalaga> ObtenerPuntuacionesUsuarios()
        {
            return _galagaSP.ObtenerPuntuacionesUsuarios();
        }
    }
}
