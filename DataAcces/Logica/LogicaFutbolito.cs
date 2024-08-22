using DataAcces.DataAccess.SPs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAcces.Logica
{
    public class LogicaFutbolito
    {
        private readonly FutbolitoSP _futbolitoSP;

        // Constructor que inyecta la dependencia de la clase de SPs
        public LogicaFutbolito(FutbolitoSP futbolitoSP)
        {
            _futbolitoSP = futbolitoSP;
        }


        public bool CrearPartida(long identificadorUsuario1, out long identificadorJuego, out string descripcionError)
        {
            return _futbolitoSP.CrearPartida(identificadorUsuario1, out identificadorJuego, out descripcionError);
        }

        public bool BuscarPartida(
            long identificadorUsuario,
            out long identificadorJuego,
            out int identificadorTurno,
            out long identificadorUsuario1,
            out string nombreUsuario1,
            out string descripcionError)
        {
            return _futbolitoSP.BuscarPartida(
                identificadorUsuario,
                out identificadorJuego,
                out identificadorTurno,
                out identificadorUsuario1,
                out nombreUsuario1,
                out descripcionError);
        }



        // Método que llama al SP a través de la clase de acceso a datos (SP)
        public int ObtenerPuntuacionFutbolitoUsuario(long usuarioId)
        {
            // Se llama al método en FutbolitoSP que interactúa con el SP en la base de datos
            return _futbolitoSP.ObtenerPuntuacionFutbolitoUsuario(usuarioId);
        }
    }
}
