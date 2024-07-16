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
        public IActionResult Create([FromBody] Reserva reserva)
        {
            if (!Email.VerifyMail(reserva.EmailCliente))
                return BadRequest($"O email \"{reserva.EmailCliente}\" é inválido");
            
            if (!Phone.VerifyPhone(reserva.TelefoneCliente, out string telefoneFormatado))
                return BadRequest($"O telefone \"{reserva.TelefoneCliente}\" é inválido.");

            reserva.TelefoneCliente = telefoneFormatado;

            var verificaMesa = _context.Mesas.FirstOrDefault(m => m.Id == reserva.IdMesa);

            if (verificaMesa == null)
                return BadRequest("Mesa inválida.");
            
            var mesaReservada = _context.Reservas.First(m => m.IdMesa == reserva.IdMesa && m.DataHoraReserva == reserva.DataHoraReserva);
            
            if (mesaReservada != null)
                return BadRequest("A mesa já está reservada.");

            _context.Reservas.Add(reserva);
            _context.SaveChanges();
            return Ok(reserva);
        }
    }
}