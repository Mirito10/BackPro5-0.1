using DataAcces.DataAccess.SPs;
using DataAcces.Entities;

public class LogicaAmistad
{
    private readonly AmistadSP _amistadSP;

    public LogicaAmistad(AmistadSP amistadSP)
    {
        _amistadSP = amistadSP;
    }

    public bool IniciarAmistad(long usuarioId1, long usuarioId2, out int identificador, out string descripcionError)
    {
        return _amistadSP.IniciarAmistad(usuarioId1, usuarioId2, out identificador, out descripcionError);
    }
    public bool EliminarAmistad(long usuarioId1, long usuarioId2, out int estatus, out string descripcionError)
    {
        return _amistadSP.EliminarAmistad(usuarioId1, usuarioId2, out estatus, out descripcionError);
    }
    public List<AmistadUsuario> ListarAmistades(long usuarioId, out string descripcionError)
    {
        return _amistadSP.ListarAmistades(usuarioId, out descripcionError);
    }
}
