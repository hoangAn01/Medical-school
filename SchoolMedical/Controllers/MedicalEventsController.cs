using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolMedical.Core.DTOs.MedicalEvent;
using SchoolMedical.Core.Entities;
using SchoolMedical.Infrastructure.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolMedical.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MedicalEventsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MedicalEventsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MedicalEventDto>>> GetMedicalEvents()
        {
            var events = await _context.MedicalEvents
                .Include(me => me.Student)
                .Select(me => new MedicalEventDto
                {
                    EventID = me.EventID,
                    StudentID = me.StudentID,
                    StudentName = me.Student.FullName,
                    EventType = me.EventType,
                    EventTime = me.EventTime,
                    Description = me.Description
                })
                .ToListAsync();

            return Ok(events);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMedicalEvent([FromBody] MedicalEventCreateDto dto)
        {
            var medicalEvent = new MedicalEvent
            {
                StudentID = dto.StudentID,
                EventType = dto.EventType,
                Description = dto.Description,
                EventTime = dto.EventTime,
                NurseID = dto.NurseID
            };
            _context.MedicalEvents.Add(medicalEvent);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetMedicalEventById), new { id = medicalEvent.EventID }, medicalEvent);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MedicalEventDto>> GetMedicalEventById(int id)
        {
            var medicalEvent = await _context.MedicalEvents
                .Include(me => me.Student)
                .Where(me => me.EventID == id)
                .Select(me => new MedicalEventDto
                {
                    EventID = me.EventID,
                    StudentID = me.StudentID,
                    StudentName = me.Student.FullName,
                    EventType = me.EventType,
                    EventTime = me.EventTime,
                    Description = me.Description
                })
                .FirstOrDefaultAsync();

            if (medicalEvent == null)
                return NotFound();

            return Ok(medicalEvent);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMedicalEvent(int id)
        {
            var medicalEvent = await _context.MedicalEvents.FindAsync(id);
            if (medicalEvent == null)
                return NotFound();

            _context.MedicalEvents.Remove(medicalEvent);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMedicalEvent(int id, [FromBody] MedicalEventUpdateDto dto)
        {
            var medicalEvent = await _context.MedicalEvents.FindAsync(id);
            if (medicalEvent == null)
                return NotFound();

            medicalEvent.EventType = dto.EventType;
            medicalEvent.Description = dto.Description;
            medicalEvent.EventTime = dto.EventTime;
            medicalEvent.NurseID = dto.NurseID;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<MedicalEventDto>>> SearchMedicalEvents(
            [FromQuery] string? keyword)
        {
            var query = _context.MedicalEvents.Include(me => me.Student).AsQueryable();

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(me =>
                    me.Student.FullName.Contains(keyword) ||
                    me.StudentID.ToString().Contains(keyword));
            }

            var events = await query
                .Select(me => new MedicalEventDto
                {
                    EventID = me.EventID,
                    StudentID = me.StudentID,
                    StudentName = me.Student.FullName,
                    EventType = me.EventType,
                    EventTime = me.EventTime,
                    Description = me.Description
                })
                .ToListAsync();

            return Ok(events);
        }
    }

    public class MedicalEventUpdateDto
    {
        public string? EventType { get; set; }
        public string? Description { get; set; }
        public DateTime EventTime { get; set; }
        public int? NurseID { get; set; }
    }
}
