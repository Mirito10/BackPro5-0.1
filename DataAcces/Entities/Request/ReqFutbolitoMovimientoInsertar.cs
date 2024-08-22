
namespace DataAcces.Entities
{
    public class ReqFutbolitoMovimientoInsertar
    {
        public long? identificadorTurno { get; set; }
        public long identificadorUsuario { get; set; }
        public float direccionX { get; set; }
        public float direccionY { get; set; }
        public int tipoJugador { get; set; }
    }
}
