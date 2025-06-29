using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolMedical.Core.Entities;
using SchoolMedical.Infrastructure.Data;

[ApiController]
[Route("api/[controller]")]
public class VaccineRecordController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public VaccineRecordController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var records = await _context.VaccineRecords
            .Include(r => r.Student)
            .Include(r => r.VaccinationEvent)
            .Include(r => r.Nurse)
            .ToListAsync();
        return Ok(records);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var record = await _context.VaccineRecords
            .Include(r => r.Student)
            .Include(r => r.VaccinationEvent)
            .Include(r => r.Nurse)
            .FirstOrDefaultAsync(r => r.VaccineRecordID == id);

        if (record == null) return NotFound();
        return Ok(record);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] VaccineRecord model)
    {
        _context.VaccineRecords.Add(model);
        await _context.SaveChangesAsync();

        var student = await _context.Students.Include(s => s.Parent)
            .FirstOrDefaultAsync(s => s.StudentID == model.StudentID);
        if (student?.Parent != null)
        {
            var noti = new Notification{
                Title = $"Kết quả tiêm {model.VaccineName}",
                Content = $"Học sinh {student.FullName} đã được tiêm {model.VaccineName} ngày {model.InjectionDate:dd/MM/yyyy}.",
                SentDate = DateTime.Now,
                Status = "Sent",
                NotificationType = "VaccineResult",
                VaccinationEventID = model.VaccinationEventID
            };
            var pn = new ParentNotification{
                ParentID = student.Parent.ParentID,
                Notification = noti,
                IndividualStatus = "Sent",
                IndividualSentDate = DateTime.Now
            };
            _context.AddRange(noti, pn);
            await _context.SaveChangesAsync();
        }

        return CreatedAtAction(nameof(GetById), new { id = model.VaccineRecordID }, model);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] VaccineRecord model)
    {
        if (id != model.VaccineRecordID) return BadRequest();

        _context.Entry(model).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var record = await _context.VaccineRecords.FindAsync(id);
        if (record == null) return NotFound();

        _context.VaccineRecords.Remove(record);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
