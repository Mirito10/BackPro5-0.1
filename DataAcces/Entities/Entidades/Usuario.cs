
namespace DataAcces.Entities
{
    public class Usuario
    {
        public string nombre { get; set; }
        public DateTime fechaNacimiento { get; set; }
        public string correo { get; set; }
        public string contrasena { get; set; }
        public Byte[] fotoPerfil { get; set; }
    }
}
