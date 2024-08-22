
namespace DataAcces.Entities
{
    public class ReqGalagaPartidaInsertar
    {
        public GalagaCrearPartidaUser user { get; set; }

        public class GalagaCrearPartidaUser
        {
            public long identificadorUsuario { get; set; }
            public int puntajeUsuario { get; set; }
            public float duracion { get; set; }
        }
    }
}
