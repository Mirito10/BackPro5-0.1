using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAcces.Entities;

namespace DataAcces.DataAccess.SPs
{
    public class AmistadSP
    {
        private readonly DatabaseHelper _dbHelper;

        public AmistadSP(DatabaseHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }
        public bool IniciarAmistad(long usuarioId1, long usuarioId2, out int identificador, out string descripcionError)
        {
            identificador = 0;
            descripcionError = string.Empty;

            try
            {
                using (var connection = _dbHelper.GetConnection())
                {
                    using (var command = new SqlCommand("SP_UsuarioAmistadIniciar", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@V_identificadorUsuario1", usuarioId1);
                        command.Parameters.AddWithValue("@V_identificadorUsuario2", usuarioId2);

                        var identificadorParam = new SqlParameter("@V_Identificador", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output
                        };
                        command.Parameters.Add(identificadorParam);

                        var descripcionErrorParam = new SqlParameter("@V_DescripcionError", SqlDbType.NVarChar, 255)
                        {
                            Direction = ParameterDirection.Output
                        };
                        command.Parameters.Add(descripcionErrorParam);

                        connection.Open();
                        command.ExecuteNonQuery();

                        identificador = (int)identificadorParam.Value;
                        descripcionError = descripcionErrorParam.Value.ToString();

                        return string.IsNullOrEmpty(descripcionError);
                    }
                }
            }
            catch (Exception ex)
            {
                descripcionError = "Error al iniciar la amistad: " + ex.Message;
                return false;
            }
        }

        public List<AmistadUsuario> ListarAmistades(long identificacionUsuario, out string descripcionError)
        {
            List<AmistadUsuario> amistades = new List<AmistadUsuario>();
            descripcionError = string.Empty;

            try
            {
                using (var connection = _dbHelper.GetConnection())
                {
                    using (var command = new SqlCommand("SP_UsuarioAmistadListar", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@V_identificacionUsuario", identificacionUsuario);


                        connection.Open();

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var amistad = new AmistadUsuario
                                {
                                    nombreUsuario1 = reader.GetString(reader.GetOrdinal("nombreUsuario1")),
                                    nombreUsuario2 = reader.GetString(reader.GetOrdinal("nombreUsuario2")),
                                    fechaInicio = reader.GetDateTime(reader.GetOrdinal("fechaInicio"))
                                };
                                amistades.Add(amistad);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                descripcionError = "Error al listar amistades: " + ex.Message;
            }

            return amistades;
        }

        public bool EliminarAmistad(long usuarioId1, long usuarioId2, out int estatus, out string descripcionError)
        {
            estatus = 0;
            descripcionError = string.Empty;

            try
            {
                using (var connection = _dbHelper.GetConnection())
                {
                    using (var command = new SqlCommand("SP_UsuarioAmistadEliminar", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Añadir parámetros
                        command.Parameters.AddWithValue("@V_identificacionUsuario1", usuarioId1);
                        command.Parameters.AddWithValue("@V_identificacionUsuario2", usuarioId2);

                        var estatusParam = new SqlParameter("@V_Estatus", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output
                        };
                        command.Parameters.Add(estatusParam);

                        var descripcionErrorParam = new SqlParameter("@V_DescripcionError", SqlDbType.NVarChar, 255)
                        {
                            Direction = ParameterDirection.Output
                        };
                        command.Parameters.Add(descripcionErrorParam);

                        connection.Open();
                        command.ExecuteNonQuery();

                        estatus = (int)estatusParam.Value;
                        descripcionError = descripcionErrorParam.Value.ToString();


                        if (estatus == 0)
                        {
                            return true;
                        }
                        else
                        {
                            descripcionError = string.IsNullOrEmpty(descripcionError)
                               ? "Operación fallida, pero no se proporcionó un mensaje de error."
                               : descripcionError;
                        }


                        return estatus == 1;
                    }
                }
            }
            catch (Exception ex)
            {
                descripcionError = "Error al eliminar la amistad: " + ex.Message;
                return false;
            }
        }




    }
}
