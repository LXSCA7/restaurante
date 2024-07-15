using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Restaurante.Api.Models
{
    public class Reserva
    {
        public int Id { get; set; }
        public int IdMesa { get; set; }
        public int IdUsuario { get; set; }
        public Usuario Usuario { get; set; }
        public Mesa Mesa { get; set; }
        public DateTime DataReserva { get; set; }
        public TimeSpan HoraReserva { get; set; }
    }
}