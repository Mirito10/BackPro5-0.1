using DataAcces.Entities;
using Logica;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly LogicaUsuario _logicaUsuario;

        public UsuarioController(LogicaUsuario logicaUsuario)
        {
            _logicaUsuario = logicaUsuario;
        }

        [HttpGet]
        [Route("Verificar")]
        public IActionResult VerificarUsuario([FromQuery] string correo, [FromQuery] string contrasena)
        {
            var req = new ReqUsuarioVerificar
            {
                correo = correo,
                contrasena = contrasena
            };

            var resultado = _logicaUsuario.VerificarUsuario(req);

            if (!resultado.resultado)
            {
                return Ok("Usuario verificado exitosamente.");  // 200 OK
            }
            else
            {
                return BadRequest(resultado.descripcionError);  // 400 Bad Request
            }
        }
        [HttpPost]
        [Route("Verificar")]
        public IActionResult VerificarUsuario([FromBody] ReqUsuarioVerificar req)
        {
            var resultado = _logicaUsuario.VerificarUsuario(req);

            if (!resultado.resultado)
            {
                return Ok("Usuario verificado exitosamente.");  // 200 OK
            }
            else
            {
                return BadRequest(resultado.descripcionError);  // 400 Bad Request
            }
        }

        [HttpPost]
        [Route("Crear")]
        public IActionResult CrearUsuario([FromBody] ReqUsuarioCrear req)
        {
            var resultado = _logicaUsuario.CrearUsuario(req);

            if (!resultado.resultado)
            {
                return BadRequest(resultado.descripcionError);  // 400 Bad Request si ocurre un error
            }
            else
            {
                return Ok("Usuario creado exitosamente.");  // 200 OK si se crea correctamente
            }
        }

    }
}
