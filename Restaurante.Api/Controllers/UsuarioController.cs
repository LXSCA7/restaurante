using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Restaurante.Api.Context;

namespace Restaurante.Api.Controllers
{
    public class UsuarioController
    {
        private readonly ApplicationDbContext _context;
        public UsuarioController(ApplicationDbContext context)
        {
            _context = context;
        }
    }
}