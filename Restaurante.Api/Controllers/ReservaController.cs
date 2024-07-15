using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Restaurante.Api.Context;
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
            return Ok(reserva);
        }
    }
}