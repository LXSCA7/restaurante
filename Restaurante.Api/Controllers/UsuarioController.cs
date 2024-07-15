using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
    public class UsuarioController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public UsuarioController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPut("criar-user")]
        public IActionResult Create([FromBody] Usuario user)
        {
            var phoneUtil = new Phone();
            if (!VerifyMail(user.Email))
                return BadRequest($"O email \"{user.Email}\" é inválido");
            
            if (!phoneUtil.VerifyPhone(user.Telefone, out string telefoneFormatado))
                return BadRequest($"O telefone \"{user.Telefone}\" é inválido.");

            _context.Usuarios.Add(user);
            _context.SaveChanges();
            return Ok($"Usuario {user.Nome} criado com sucesso.");
        }

        // funcoes
        private bool VerifyMail(string email)
        {
            if (email.Any(c => c == '@') && email.Length > 1)
                return true;
            
            return false;
        }
    }
}