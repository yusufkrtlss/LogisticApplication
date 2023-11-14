using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LogisticApi.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Security.Cryptography.Pkcs;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LogisticApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	//[Authorize]
	public class RolesController : Controller
    {
        private readonly UzserLojistikContext _context;

        public RolesController(UzserLojistikContext context)
        {
            _context = context;
        }

		// GET: api/Roles
		[HttpGet]
		public async Task<ActionResult<IEnumerable<Role>>> GetRoles()
		{
			//try
			//{
			//    var jsonSerializerOptions = new JsonSerializerOptions
			//    {
			//        ReferenceHandler = ReferenceHandler.Preserve,
			//        // Diğer JsonSerializerOptions ayarlarını da buraya ekleyebilirsiniz
			//    };

			//    var rolesWithModules = await _context.Roles
			//        .Include(r => r.RoleModules) // Bu satır, ilişkilendirilmiş modüllerin de çekilmesini sağlar
			//        .Where(r => r.IsDeleted != true) // Silinmemiş rolleri filtrele
			//        .ToListAsync();

			//    var jsonString = JsonSerializer.Serialize(rolesWithModules, jsonSerializerOptions);

			//    return Ok(jsonString);
			//}
			//catch (Exception ex)
			//{
			//    // Hata durumunda isteği işleyin ve uygun bir hata mesajı döndürün
			//    return StatusCode(500, $"Internal server error: {ex.Message}");
			//}
			if (_context.Roles == null)
			{
				return BadRequest("Tanımlı rol listesi okunamadı");
			}
			var _LoginUser = HttpContext.User;
			return await _context.Roles.ToListAsync();
		}
        
      
        [HttpGet("{id}")]

		public async Task<ActionResult<Role>> GetRole(Guid id)
		{
            //try
            //{
            //    var jsonSerializerOptions = new JsonSerializerOptions
            //    {
            //        ReferenceHandler = ReferenceHandler.Preserve,
            //        // Diğer JsonSerializerOptions ayarlarını da buraya ekleyebilirsiniz
            //    };

            //    var rolesWithModules = await _context.Roles
            //        .Where(r => r.Id == id && r.IsDeleted != true) // Belirli bir id'ye sahip ve silinmemiş rolü filtrele
            //        .Include(r => r.RoleModules) // Bu satır, ilişkilendirilmiş modüllerin de çekilmesini sağlar
            //        .ToListAsync();

            //    var jsonString = JsonSerializer.Serialize(rolesWithModules, jsonSerializerOptions);

            //    return Ok(jsonString);
            //}
            //catch (Exception ex)
            //{
            //    // Hata durumunda isteği işleyin ve uygun bir hata mesajı döndürün
            //    return StatusCode(500, $"Internal server error: {ex.Message}");
            
            if (_context.Roles == null)
			{
				return BadRequest("Tanımlı rol listesi okunamadı");
			}
			var role = await _context.Roles.FindAsync(id);

			if (role == null)
			{
				return BadRequest("İstenilen rol kaydı bulunamadı");
			}

            return role;
		}

		// PUT: api/Roles/aef54-guh567-
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPut("{id}")]

		public async Task<IActionResult> PutRole(Guid id, Role role)
		{
			if (id != role.Id)
			{
				return BadRequest("Güncellenecek rol anahtarı uyumsuz");
			}

			var _LoginUser = HttpContext.User;
			role.LastUpdated = DateTime.Now;
			_context.Entry(role).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!RoleExists(role.Id))
				{
					return BadRequest("Güncellenecek rol kaydı bulunamadı");
				}
				else
				{
					throw;
				}
			}

			return NoContent();
		}

		// POST: api/Roles
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPost]

		public async Task<ActionResult<Role>> PostRole(Role role)
		{
			var _LoginUser = HttpContext.User;
			if (_context.Roles == null)
			{
				return BadRequest("Tanımlı rol listesi okunamadı");
			}
			if (role.Id.ToString() == string.Empty)
			{
				role.Id = new Guid();
			}
			_context.Roles.Add(role);
			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateException)
			{
				if (RoleExists(role.Id))
				{
					return BadRequest("Gönderilen rol kayıtlı. PUT yöntemi ile güncelleme gönderilmelidir");
				}
				else
				{
					throw;
				}
			}

			return CreatedAtAction("GetRole", new { id = role.Id }, role);
		}

		// DELETE: api/Roles/5
		[HttpDelete("{id}")]

		public async Task<IActionResult> DeleteRole(Guid id)
		{
			var _LoginUser = HttpContext.User;
			if (_context.Roles == null)
			{
				return BadRequest("Tanımlı rol listesi okunamadı");
			}
			var role = await _context.Roles.FindAsync(id);
			if (role == null)
			{
				return BadRequest("Silinecek rol kaydı bulunamadı");
			}
			role.IsDeleted = true;
			role.LastUpdated = DateTime.Now;

			_context.Roles.Update(role);
			await _context.SaveChangesAsync();

			return NoContent();
		}

		private bool RoleExists(Guid id)
        {
          return (_context.Roles?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
