using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Restaurante.Api.Models
{
    public class ReservaBody
    {
        public string NomeCliente { get; set; }
        public string EmailCliente { get; set; }
        public string TelefoneCliente { get; set; }
        public DateTime DataHoraReserva { get; set; }
        public int QuantidadeDePessoas { get; set; }
    }
}