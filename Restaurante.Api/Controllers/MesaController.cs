using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Restaurante.Api.Context;

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
    }
}