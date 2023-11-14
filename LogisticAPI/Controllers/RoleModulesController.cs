using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LogisticApi.Models;

namespace LogisticApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleModulesController : ControllerBase
    {
        private readonly UzserLojistikContext _context;

        public RoleModulesController(UzserLojistikContext context)
        {
            _context = context;
        }

        // GET: api/RoleModules
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RoleModule>>> GetRoleModules()
        {
          if (_context.RoleModules == null)
          {
              return NotFound();
          }
            return await _context.RoleModules.ToListAsync();
        }

        // GET: api/RoleModules/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RoleModule>> GetRoleModule(Guid id)
        {
          if (_context.RoleModules == null)
          {
              return NotFound();
          }
            var roleModule = await _context.RoleModules.FindAsync(id);

            if (roleModule == null)
            {
                return NotFound();
            }

            return roleModule;
        }
        // GET: api/RoleModules/ByRoleId/5
        [HttpGet("ByRoleId/{roleId}")]
        public async Task<ActionResult<IEnumerable<RoleModule>>> GetRoleModuleByRoleId(Guid roleId)
        {
            if (roleId == Guid.Empty)
            {
                return NotFound();
            }

            var roleModule = await _context.RoleModules.Where(rm => rm.RoleId == roleId).ToListAsync();

            if (roleModule == null)
            {
                return NotFound();
            }

            return roleModule;
        }
        // PUT: api/RoleModules/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRoleModule(Guid id, RoleModule roleModule)
        {
            if (id != roleModule.Id)
            {
                return BadRequest();
            }

            _context.Entry(roleModule).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RoleModuleExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/RoleModules
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<RoleModule>> PostRoleModule(RoleModule roleModule)
        {
          if (_context.RoleModules == null)
          {
              return Problem("Entity set 'UzserLojistikContext.RoleModules'  is null.");
          }
            _context.RoleModules.Add(roleModule);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (RoleModuleExists(roleModule.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetRoleModule", new { id = roleModule.Id }, roleModule);
        }

        // DELETE: api/RoleModules/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoleModule(Guid id)
        {
            if (_context.RoleModules == null)
            {
                return NotFound();
            }
            var roleModule = await _context.RoleModules.FindAsync(id);
            if (roleModule == null)
            {
                return NotFound();
            }

            _context.RoleModules.Remove(roleModule);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RoleModuleExists(Guid id)
        {
            return (_context.RoleModules?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
