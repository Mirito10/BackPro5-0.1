
namespace DataAcces.Entities
{
    public class FutbolitoTurno
    {
        public long? IdentificadorPartida { get; set; }
        public int numeroTurno { get; set; }
        public int IdentificadorTurno { get; set; }
        public int? IdentificadorTurnoSiguiente { get; set; }
        public string errorDescripcion { get; set; }
        public long? IdentificadorGanador { get; set; }
    }
}
