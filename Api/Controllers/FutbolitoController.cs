using DataAcces.Entities;
using DataAcces.Logica;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FutbolitoController : ControllerBase
    {

        private readonly LogicaFutbolito _logicaFutbolito;

        public FutbolitoController(LogicaFutbolito logicaFutbolito)
        {
            _logicaFutbolito = logicaFutbolito;
        }

        [HttpPost("CrearPartida")]
        public ActionResult CrearPartida([FromBody] ReqFutbolitoPartidaCrear request)
        {
            if (_logicaFutbolito.CrearPartida(request.identificacionUsuario, out var identificadorJuego, out var descripcionError))
            {
                return Ok(new { identificadorJuego, mensaje = "Partida creada con éxito." });
            }
            else
            {
                return BadRequest(new { error = descripcionError });
            }
        }


        [HttpPost("UnirseAPartida")]
        public IActionResult UnirseAPartida([FromBody] UnirsePartidaRequest request)
        {
            var exito = _logicaFutbolito.BuscarPartida(
                request.IdentificadorUsuario,
                out long identificadorJuego,
                out int identificadorTurno,
                out long identificadorUsuario1,
                out string nombreUsuario1,
                out string descripcionError
            );

            if (exito)
            {
                return Ok(new
                {
                    mensaje = "Partida encontrada y usuario unido con éxito",
                    identificadorJuego,
                    identificadorTurno,
                    identificadorUsuario1,
                    nombreUsuario1
                });
            }
            else
            {
                return BadRequest(new { error = descripcionError });
            }
        }



        public class UnirsePartidaRequest
        {
            public long IdentificadorJuego { get; set; }
            public long IdentificadorUsuario { get; set; }
        }


        [HttpGet("ObtenerPuntuacionFutbolitoUsuario")]
        public ActionResult<int> ObtenerPuntuacionFutbolitoUsuario(long usuarioId)
        {
            try
            {
                var puntaje = _logicaFutbolito.ObtenerPuntuacionFutbolitoUsuario(usuarioId);
                return Ok(puntaje);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error: " + ex.Message);
            }
        }

    }
}
