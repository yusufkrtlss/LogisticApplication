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
	//[Authorize]
	public class CompaniesController : ControllerBase
    {
        private readonly UzserLojistikContext _context;
       
        public CompaniesController(UzserLojistikContext context)
        {
            _context = context;
        }


        // GET: api/Companies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Company>>> GetCompanies()
        {
          if (_context.Companies == null)
          {
                //return NotFound();
                return BadRequest("Tanımlı firma listesi okunamadı");
          }
            var _LoginUser = HttpContext.User;
   		   return await _context.Companies.ToListAsync();
        }

        // GET: api/Companies/5
        [HttpGet("{id}")]
		
		public async Task<ActionResult<Company>> GetCompany(Guid id)
        {
            if (_context.Companies == null)
            {
				return BadRequest("Tanımlı firma listesi okunamadı");
			}
            var company = await _context.Companies.FindAsync(id);

            if (company == null)
            {
				return BadRequest("İstenilen firma kaydı bulunamadı");
			}

            return company;
        }

        // PUT: api/Companies/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
		
		public async Task<IActionResult> PutCompany(Guid id , Company company)
        {
            if (id != company.Id)
            {
				return BadRequest("Güncellenecek firma anahtarı uyumsuz");
			}

            var _LoginUser = HttpContext.User;
			_context.Entry(company).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CompanyExists(company.Id))
                {
                    return BadRequest("Güncellenecek firma kaydı bulunamadı");
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Companies
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
		
		public async Task<ActionResult<Company>> PostCompany(Company company)
        {
			var _LoginUser = HttpContext.User;
			if (_context.Companies == null)
			{
				return BadRequest("Tanımlı firma listesi okunamadı");
			}
			if (company.Id.ToString() == string.Empty)
            {
                company.Id = new Guid();
            }
            _context.Companies.Add(company);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (CompanyExists(company.Id))
                {
                    return BadRequest("Gönderilen firma kayıtlı. PUT yöntemi ile güncelleme gönderilmelidir");
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetCompany", new { id = company.Id }, company);
        }

        // DELETE: api/Companies/5
        [HttpDelete("{id}")]
		
		public async Task<IActionResult> DeleteCompany(Guid id)
        {
			var _LoginUser = HttpContext.User;
			if (_context.Companies == null)
			{
				return BadRequest("Tanımlı firma listesi okunamadı");
			}
			var company = await _context.Companies.FindAsync(id);
            if (company == null)
            {
				return BadRequest("Silinecek firma kaydı bulunamadı");
			}
            company.IsDeleted = true;
            company.LastUpdated = DateTime.Now;
          
            _context.Companies.Update(company);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CompanyExists(Guid id)
        {
            return (_context.Companies?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
