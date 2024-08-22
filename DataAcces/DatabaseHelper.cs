using System;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

public class DatabaseHelper
{
    public string _connectionString;

    public DatabaseHelper(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    public SqlConnection GetConnection()
    {
        return new SqlConnection(_connectionString);
    }

    // Método para verificar la conexión
    public void VerificarConexion()
    {
        try
        {
            using (SqlConnection connection = GetConnection())
            {
                connection.Open();
                Console.WriteLine("La conexión a la base de datos fue exitosa.");
            }
        }
        catch (SqlException ex)
        {
            Console.WriteLine($"Error al conectar a la base de datos: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ocurrió un error inesperado: {ex.Message}");
        }
    }
}
