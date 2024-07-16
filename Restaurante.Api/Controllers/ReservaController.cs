using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Restaurante.Api.Context;
using Restaurante.Api.Functions;
using Restaurante.Api.Models;

namespace Restaurante.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservaController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public ReservaController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPut("criar-reserva")]
        public IActionResult Create([FromBody] ReservaBody reservaBase)
        {
            if (!Email.VerifyMail(reservaBase.EmailCliente))
                return BadRequest($"O email \"{reservaBase.EmailCliente}\" é inválido");
            
            if (!Phone.VerifyPhone(reservaBase.TelefoneCliente, out string telefoneFormatado))
                return BadRequest($"O telefone \"{reservaBase.TelefoneCliente}\" é inválido.");

            reservaBase.TelefoneCliente = telefoneFormatado;

            // verifica se tem mesas disponiveis de forma automatica, passando o numero (id) da mesa pro usuario
            List<Mesa> mesas = _context.Mesas.Where(c => c.Capacidade == reservaBase.QuantidadeDePessoas).ToList();
            
            int idMesaDisponivel = -1;
            if (mesas == null)
                return BadRequest("Não há mesas disponíveis.");

            foreach (Mesa mesa in mesas)
            {
                if (!_context.Reservas.Any(r => r.IdMesa == mesa.Id && r.DataHoraReserva == reservaBase.DataHoraReserva))
                {
                    idMesaDisponivel = mesa.Id;
                    break;
                }
            }

            if (idMesaDisponivel == -1)
                return BadRequest("Não temos mesas disponíveis.");

            Reserva reserva = new() {
                NomeCliente = reservaBase.NomeCliente,
                EmailCliente = reservaBase.EmailCliente,
                TelefoneCliente = reservaBase.TelefoneCliente,
                IdMesa = idMesaDisponivel,
                DataHoraReserva = reservaBase.DataHoraReserva,
                QuantidadeDePessoas = reservaBase.QuantidadeDePessoas
            };

            _context.Reservas.Add(reserva);
            _context.SaveChanges();

            // TO-DO: colocar script para enviar mensagem pro usuario

            return Ok("Sua reserva foi realizada com sucesso. Número da sua mesa:" + reserva.IdMesa);
        }
    }
}