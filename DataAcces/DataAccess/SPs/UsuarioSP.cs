using System;
using System.Data;
using System.Data.SqlClient;

namespace DataAccess.SPs
{
    public class UsuarioSP
    {
        private readonly DatabaseHelper _dbHelper;

        public UsuarioSP(DatabaseHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        // Método para verificar un usuario
        public bool VerificarUsuario(string correo, string contrasena, out string descripcionError)
        {
            descripcionError = null;
            bool resultado = false;

            try
            {
                using (var connection = _dbHelper.GetConnection())
                {
                    using (var command = new SqlCommand("SP_UsuarioVerificar", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Añadir parámetros
                        command.Parameters.AddWithValue("@V_correo", correo);
                        command.Parameters.AddWithValue("@V_contrasena", contrasena);
                        SqlParameter descripcionErrorParam = new SqlParameter("@V_DescripcionError", SqlDbType.NVarChar, 255)
                        {
                            Direction = ParameterDirection.Output
                        };
                        command.Parameters.Add(descripcionErrorParam);

                        connection.Open();

                        // Ejecutar el procedimiento almacenado
                        command.ExecuteNonQuery();

                        // Obtener el mensaje de error si existe
                        descripcionError = descripcionErrorParam.Value.ToString();

                        if (string.IsNullOrEmpty(descripcionError))
                        {
                            resultado = true;  // Usuario verificado correctamente
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                descripcionError = "Error al verificar usuario en la base de datos: " + ex.Message;
            }

            return resultado;
        }

        // Método para crear un usuario
        public bool CrearUsuario(string nombre, DateTime fechaNacimiento, string contrasena, byte[] fotoPerfil, string correo, out string descripcionError)
        {
            descripcionError = null;
            bool resultado = false;

            try
            {
                using (var connection = _dbHelper.GetConnection())
                {
                    using (var command = new SqlCommand("SP_UsuarioCrear", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Añadir parámetros
                        command.Parameters.AddWithValue("@V_Nombre", nombre);
                        command.Parameters.AddWithValue("@V_FechaNacimiento", fechaNacimiento);
                        command.Parameters.AddWithValue("@V_Contrasena", contrasena);
                        command.Parameters.AddWithValue("@V_FotoPerfil", fotoPerfil);
                        command.Parameters.AddWithValue("@V_Correo", correo);


                        // Añadir el parámetro de salida para la descripción del error
                        SqlParameter descripcionErrorParam = new SqlParameter("@V_DescripcionError", SqlDbType.NVarChar, 255)
                        {
                            Direction = ParameterDirection.Output
                        };
                        command.Parameters.Add(descripcionErrorParam);

                        connection.Open();

                        // Ejecutar el procedimiento almacenado
                        command.ExecuteNonQuery();

                        // Obtener los valores de los parámetros de salida
                        descripcionError = descripcionErrorParam.Value.ToString();

                        if (string.IsNullOrEmpty(descripcionError))
                        {
                            resultado = true;  // Usuario creado correctamente
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                descripcionError = "Error al crear el usuario en la base de datos: " + ex.Message;
            }

            return resultado;
        }
    }
}

