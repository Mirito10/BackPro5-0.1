using DataAcces.Entities;
using DataAccess.SPs;


namespace Logica
{
    public class LogicaUsuario
    {
        private readonly UsuarioSP _usuarioSP;

        public LogicaUsuario(UsuarioSP usuarioSP)
        {
            _usuarioSP = usuarioSP;
        }

        public ResUsuarioVerificar VerificarUsuario(ReqUsuarioVerificar req)
        {
            ResUsuarioVerificar res = new ResUsuarioVerificar { resultado = false };

            if (req == null)
            {
                res.descripcionError = "El request es null.";
            }
            else if (string.IsNullOrEmpty(req.correo))
            {
                res.descripcionError = "No se ingresó un correo.";
            }
            else if (string.IsNullOrEmpty(req.contrasena))
            {
                res.descripcionError = "No se ingresó una contraseña.";
            }
            else
            {
                try
                {
                    string errorDescripcion;
                    bool verificado = _usuarioSP.VerificarUsuario(req.correo, req.contrasena, out errorDescripcion);

                    if (verificado)
                    {
                        res.resultado = true;
                    }
                    else
                    {
                        res.descripcionError = string.IsNullOrEmpty(errorDescripcion) ? "Credenciales incorrectas." : errorDescripcion;
                    }
                }
                catch (Exception ex)
                {
                    res.descripcionError = "Error en la conexión a la base de datos: " + ex.Message;
                }
            }

            return res;
        }
        public ResUsuarioCrear CrearUsuario(ReqUsuarioCrear req)
        {
            ResUsuarioCrear res = new ResUsuarioCrear();

            res.resultado = false;  // Por defecto, asumimos que fallará

            if (req == null)
            {
                res.descripcionError = "El cuerpo de la solicitud está vacío.";
            }
            else if (string.IsNullOrEmpty(req.user.nombre))
            {
                res.descripcionError = "El nombre es requerido.";
            }
            else if (string.IsNullOrEmpty(req.user.correo))
            {
                res.descripcionError = "El correo es requerido.";
            }
            else if (string.IsNullOrEmpty(req.user.contrasena))
            {
                res.descripcionError = "La contraseña es requerida.";
            }
            else
            {
                try
                {
                    long identificadorUsuario = 0;
                    string errorDescripcion = null;

                    // Llamada al procedimiento almacenado
                    _usuarioSP.CrearUsuario(req.user.nombre, req.user.fechaNacimiento, req.user.contrasena, req.user.fotoPerfil, req.user.correo, out errorDescripcion);

                    if (identificadorUsuario <= 0)
                    {
                        res.resultado = true;
                    }
                    else
                    {
                        res.descripcionError = "No se pudo crear el usuario: " + errorDescripcion;
                    }
                }
                catch (Exception ex)
                {
                    res.descripcionError = "Error al conectar a la base de datos: " + ex.Message;
                }
            }

            return res;
        }

    }
}
