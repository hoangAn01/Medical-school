using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolMedical.Core.Entities;
using SchoolMedical.Infrastructure.Data;

namespace SchoolMedical.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VaccinationEventController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public VaccinationEventController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var events = await _context.VaccinationEvents
                .Include(e => e.ManagerAdmin)
                .Include(e => e.Class)
                .Include(e => e.VaccineRecords)
                .ToListAsync();
            return Ok(events);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var ev = await _context.VaccinationEvents
                .Include(e => e.ManagerAdmin)
                .Include(e => e.Class)
                .Include(e => e.VaccineRecords)
                .FirstOrDefaultAsync(e => e.EventID == id);

            if (ev == null) return NotFound();
            return Ok(ev);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] VaccinationEvent model)
        {
            _context.VaccinationEvents.Add(model);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = model.EventID }, model);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] VaccinationEvent model)
        {
            if (id != model.EventID) return BadRequest();

            _context.Entry(model).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ev = await _context.VaccinationEvents.FindAsync(id);
            if (ev == null) return NotFound();

            _context.VaccinationEvents.Remove(ev);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}