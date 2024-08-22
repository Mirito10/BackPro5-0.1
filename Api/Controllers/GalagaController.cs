using DataAcces.Entities;
using DataAcces.Logica;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GalagaController : ControllerBase
    {
        private readonly LogicaGalaga _logicaGalaga;

        public GalagaController(LogicaGalaga logicaGalaga)
        {
            _logicaGalaga = logicaGalaga;
        }

        [HttpPost("CrearPartida")]
        public IActionResult CrearPartida([FromBody] ReqGalagaPartidaInsertar req)
        {
            ResGalagaPartidaInsertar resultado = _logicaGalaga.CrearPartida(req);

            if (!resultado.resultado)
            {
                return BadRequest(resultado.descripcionError);
            }

            return Ok(new { mensaje = "Partida creada exitosamente.", identificadorJuego = resultado.identificadorJuego });
        }

        [HttpGet("ObtenerPuntuacionesUsuarios")]
        public ActionResult<List<PartidaGalaga>> ObtenerPuntuacionesUsuarios()
        {
            try
            {
                var puntuaciones = _logicaGalaga.ObtenerPuntuacionesUsuarios();
                return Ok(puntuaciones);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error: " + ex.Message);
            }
        }
        [HttpGet("ObtenerPuntuacionUsuario")]
        public ActionResult<List<PartidaGalaga>> ObtenerPuntuacionUsuario(long usuarioId)
        {
            try
            {
                var puntuaciones = _logicaGalaga.ObtenerPuntuacionUsuario(usuarioId);
                return Ok(puntuaciones);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error: " + ex.Message);
            }
        }


    }

}
