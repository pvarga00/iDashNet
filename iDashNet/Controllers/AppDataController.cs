using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using iDashNet.Models;

namespace iDashNet.Controllers
{
    [Produces("application/json")]
    [Route("api/AppData")]
    public class AppDataController : Controller
    {
        private readonly iDashContext _context;

        public AppDataController(iDashContext context)
        {
            _context = context;
        }

        // GET: api/AppData
        [HttpGet]
        public IEnumerable<AppData> GetAppData()
        {
            return _context.AppData;
        }

        // GET: api/AppData/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAppData([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var appData = await _context.AppData.SingleOrDefaultAsync(m => m.ID == id);

            if (appData == null)
            {
                return NotFound();
            }

            return Ok(appData);
        }

        // PUT: api/AppData/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAppData([FromRoute] int id, [FromBody] AppData appData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != appData.ID)
            {
                return BadRequest();
            }

            _context.Entry(appData).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AppDataExists(id))
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

        // POST: api/AppData
        [HttpPost]
        public async Task<IActionResult> PostAppData([FromBody] AppData appData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.AppData.Add(appData);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAppData", new { id = appData.ID }, appData);
        }

        // DELETE: api/AppData/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAppData([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var appData = await _context.AppData.SingleOrDefaultAsync(m => m.ID == id);
            if (appData == null)
            {
                return NotFound();
            }

            _context.AppData.Remove(appData);
            await _context.SaveChangesAsync();

            return Ok(appData);
        }

        private bool AppDataExists(int id)
        {
            return _context.AppData.Any(e => e.ID == id);
        }
    }
}