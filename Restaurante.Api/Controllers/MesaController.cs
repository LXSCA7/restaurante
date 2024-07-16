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
    public class MesaController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public MesaController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPut]
        public IActionResult CreateMesa(Mesa mesa)
        {
            _context.Mesas.Add(mesa);
            _context.SaveChanges();

            return Ok("Mesa criada com sucesso.");
        }

        [HttpGet]
        public IActionResult ListAll()
        {
            List<Mesa> mesas = _context.Mesas.ToList();
            return Ok(mesas);
        }
    }
}