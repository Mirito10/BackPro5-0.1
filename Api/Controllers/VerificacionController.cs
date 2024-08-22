using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class VerificacionController : ControllerBase
{
    private readonly DatabaseHelper _databaseHelper;

    public VerificacionController(DatabaseHelper databaseHelper)
    {
        _databaseHelper = databaseHelper;
    }

    [HttpGet]
    [Route("VerificarConexion")]
    public IActionResult VerificarConexion()
    {
        _databaseHelper.VerificarConexion();
        return Ok("Verificación completa. Revisa la consola para más detalles.");
    }
}
