using DataAcces.DataAccess.SPs;
using Logica;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DataAcces.Entities;



namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AmistadController : ControllerBase
    {
        private readonly LogicaAmistad _logicaAmistad;

        public AmistadController(LogicaAmistad logicaAmistad)
        {
            _logicaAmistad = logicaAmistad;
        }

        [HttpPost("IniciarAmistad")]
        public ActionResult IniciarAmistad([FromBody] AmistadUsuario solicitud)
        {
            try
            {
                int identificador;
                string descripcionError;

                // Llama a tu lógica para iniciar la amistad
                var exito = _logicaAmistad.IniciarAmistad(
                    solicitud.identificacionUsuario1.Value,
                    solicitud.identificacionUsuario2.Value,
                    out identificador,
                    out descripcionError
                );

                if (exito)
                {
                    return Ok(new { Identificador = identificador, Mensaje = "Amistad iniciada exitosamente." });
                }
                else
                {
                    return BadRequest(new { Mensaje = "Error al iniciar la amistad: " + descripcionError });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error: " + ex.Message);
            }
        }

        [HttpGet("ListarAmistades")]
        public IActionResult ListarAmistades(long usuarioId)
        {
            string descripcionError;
            var amistades = _logicaAmistad.ListarAmistades(usuarioId, out descripcionError);

            if (amistades != null && amistades.Count > 0)
            {
                return Ok(amistades);
            }
            else if (!string.IsNullOrEmpty(descripcionError))
            {
                return BadRequest(new { error = descripcionError });
            }
            else
            {
                return NotFound("No se encontraron amistades.");
            }
        }

        [HttpPost("EliminarAmistad")]
        public ActionResult EliminarAmistad([FromBody] AmistadUsuario solicitud)
        {
            int estatus;
            string descripcionError;

            // Verifica si identificadorUsuario1 y identificadorUsuario2 tienen un valor.
            if (!solicitud.identificacionUsuario1.HasValue || !solicitud.identificacionUsuario2.HasValue)
            {
                return BadRequest("Los identificadores de usuario no pueden ser null.");
            }

            long usuarioId1 = solicitud.identificacionUsuario1.Value;
            long usuarioId2 = solicitud.identificacionUsuario2.Value;

            if (_logicaAmistad.EliminarAmistad(usuarioId1, usuarioId2, out estatus, out descripcionError))
            {
                return Ok(new { Estatus = estatus, Mensaje = "Amistad eliminada exitosamente." });
            }
            else
            {
                return BadRequest(new { Estatus = estatus, Error = descripcionError });
            }
        }

    }


}

