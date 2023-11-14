using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LogisticApi.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Security.Cryptography.Pkcs;

namespace LogisticApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
	public class ModulesController : ControllerBase
    {
        private readonly UzserLojistikContext _context;

        public ModulesController(UzserLojistikContext context)
        {
            _context = context;
        }

        // GET: api/Modules
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Module>>> GetModules()
        {
           
   		   return await _context.Modules.OrderBy(m=>m.ScreenOrder).ToListAsync();
        }


    }
}
