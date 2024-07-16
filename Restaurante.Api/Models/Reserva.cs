using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Restaurante.Api.Models
{
    public class Reserva
    {
        public int Id { get; set; }

        [ForeignKey("Mesa")]
        public int IdMesa { get; set; }

        public DateTime DataHoraReserva { get; set; }

        public string NomeCliente { get; set; }
        public string EmailCliente { get; set; }
        public string TelefoneCliente { get; set; }
        public int QuantidadeDePessoas { get; set; }
    }
}