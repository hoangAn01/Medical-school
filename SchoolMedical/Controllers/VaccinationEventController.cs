using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolMedical.Core.Entities;
using SchoolMedical.Infrastructure.Data;
using System.Collections.Generic;
using System.Linq;

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
                    .ThenInclude(vr => vr.Student)
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
                    .ThenInclude(vr => vr.Student)
                .FirstOrDefaultAsync(e => e.EventID == id);

            if (ev == null) return NotFound();
            return Ok(ev);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] VaccinationEvent model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.VaccinationEvents.Add(model);
            await _context.SaveChangesAsync();

            // Nếu có ClassID, tự động tạo thông báo cho phụ huynh của học sinh trong lớp
            if (model.ClassID.HasValue)
            {
                var students = await _context.Students
                    .Where(s => s.ClassID == model.ClassID)
                    .Include(s => s.Parent)
                    .ToListAsync();

                foreach (var student in students)
                {
                    if (student.Parent != null)
                    {
                        // 1. Tạo một Notification object
                        var notification = new Notification
                        {
                            Title = $"Thông báo sự kiện tiêm chủng: {model.EventName}",
                            Content = $"Trường tổ chức tiêm chủng {model.EventName} vào ngày {model.Date:dd/MM/yyyy} tại {model.Location}. Vui lòng xác nhận đồng ý cho con tham gia.",
                            SentDate = DateTime.Now,
                            Status = "Sent",
                            NotificationType = "Event",
                            VaccinationEventID = model.EventID
                        };
                        _context.Notifications.Add(notification);
                        await _context.SaveChangesAsync(); // Lưu để lấy NotificationID

                        // 2. Tạo ParentNotification để nối Parent và Notification
                        var parentNotification = new ParentNotification
                        {
                            ParentID = student.Parent.ParentID,
                            NotificationID = notification.NotificationID, // Sử dụng ID vừa tạo
                            IndividualStatus = "Sent",
                            IndividualSentDate = DateTime.Now
                        };
                        _context.ParentNotifications.Add(parentNotification);

                        // 3. Tạo bản ghi đồng ý của phụ huynh
                        var consent = new ParentalConsent
                        {
                            StudentID = student.StudentID,
                            VaccinationEventID = model.EventID, // Sửa tên trường
                            ParentID = student.Parent.ParentID,
                            ConsentStatus = "Chờ phản hồi",     // Sửa tên trường
                            ConsentDate = DateTime.Now          // Sửa tên trường
                        };
                        _context.ParentalConsents.Add(consent);
                    }
                }
                await _context.SaveChangesAsync();
            }

            return CreatedAtAction(nameof(GetById), new { id = model.EventID }, model);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] VaccinationEvent model)
        {
            if (id != model.EventID) return BadRequest();
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                _context.Entry(model).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                // Cập nhật thông báo cho phụ huynh nếu có thay đổi ngày hoặc địa điểm
                var existingEvent = await _context.VaccinationEvents.AsNoTracking().FirstOrDefaultAsync(e => e.EventID == id);
                if (existingEvent != null && (existingEvent.Date != model.Date || existingEvent.Location != model.Location))
                {
                    var consents = await _context.ParentalConsents
                        .Where(c => c.VaccinationEventID == id) // Sửa tên trường
                        .Include(c => c.Student)
                            .ThenInclude(s => s.Parent)
                        .ToListAsync();

                    foreach (var consent in consents)
                    {
                        if (consent.Student?.Parent != null)
                        {
                            var notification = new Notification
                            {
                                Title = $"Cập nhật sự kiện tiêm chủng: {model.EventName}",
                                Content = $"Thông tin sự kiện tiêm chủng đã được cập nhật: {model.EventName} sẽ diễn ra vào ngày {model.Date:dd/MM/yyyy} tại {model.Location}.",
                                SentDate = DateTime.Now,
                                Status = "Sent",
                                NotificationType = "EventUpdate",
                                VaccinationEventID = model.EventID
                            };
                            _context.Notifications.Add(notification);
                            await _context.SaveChangesAsync();

                            var parentNotification = new ParentNotification
                            {
                                ParentID = consent.Student.Parent.ParentID,
                                NotificationID = notification.NotificationID,
                                IndividualStatus = "Sent",
                                IndividualSentDate = DateTime.Now
                            };
                            _context.ParentNotifications.Add(parentNotification);
                        }
                    }
                    await _context.SaveChangesAsync();
                }

                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.VaccinationEvents.AnyAsync(e => e.EventID == id))
                    return NotFound();
                throw;
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ev = await _context.VaccinationEvents
                .Include(e => e.VaccineRecords)
                .Include(e => e.Class)
                .FirstOrDefaultAsync(e => e.EventID == id);

            if (ev == null) return NotFound();

            // Xóa các bản ghi liên quan
            var consents = await _context.ParentalConsents
                .Where(c => c.VaccinationEventID == id) // Sửa tên trường
                .ToListAsync();
            _context.ParentalConsents.RemoveRange(consents);

            _context.VaccinationEvents.Remove(ev);
            await _context.SaveChangesAsync();

            // Thông báo cho phụ huynh về việc hủy sự kiện
            if (ev.ClassID.HasValue)
            {
                var students = await _context.Students
                    .Where(s => s.ClassID == ev.ClassID)
                    .Include(s => s.Parent)
                    .ToListAsync();

                foreach (var student in students)
                {
                    if (student.Parent != null)
                    {
                        var notification = new Notification
                        {
                            Title = $"Hủy sự kiện tiêm chủng: {ev.EventName}",
                            Content = $"Sự kiện tiêm chủng {ev.EventName} đã bị hủy.",
                            SentDate = DateTime.Now,
                            Status = "Sent",
                            NotificationType = "EventCancellation",
                            VaccinationEventID = ev.EventID
                        };
                        _context.Notifications.Add(notification);
                        await _context.SaveChangesAsync();

                        var parentNotification = new ParentNotification
                        {
                            ParentID = student.Parent.ParentID,
                            NotificationID = notification.NotificationID,
                            IndividualStatus = "Sent",
                            IndividualSentDate = DateTime.Now
                        };
                        _context.ParentNotifications.Add(parentNotification);
                    }
                }
                await _context.SaveChangesAsync();
            }

            return NoContent();
        }
        // Lấy danh sách học sinh đã được phụ huynh đồng ý cho tiêm
        [HttpGet("{eventId}/approved-students")]
        public async Task<IActionResult> GetApprovedStudents(int eventId)
        {
            var approvedConsents = await _context.ParentalConsents
                .Where(c => c.VaccinationEventID == eventId && c.ConsentStatus == "Đồng ý")
                .Include(c => c.Student)
                .ToListAsync();

            // Fetch health profiles separately since Student does not have a direct HealthProfile property
            var studentIds = approvedConsents.Select(c => c.Student.StudentID).ToList();
            var healthProfiles = await _context.HealthProfiles
                .Where(hp => studentIds.Contains((int)hp.StudentID))// CHO NAY CON COPILOT XAM LOZ
                .ToDictionaryAsync(hp => hp.StudentID);

            var students = approvedConsents.Select(c =>
            {
                healthProfiles.TryGetValue(c.Student.StudentID, out var healthProfile);
                return new
                {
                    c.Student.StudentID,
                    c.Student.FullName,
                    c.Student.DateOfBirth,
                    HealthProfile = healthProfile != null ? new
                    {
                        healthProfile.ChronicDisease,
                        healthProfile.Allergy
                    } : null
                };
            }).ToList();

            return Ok(students);
        }

        // Thống kê tình trạng đồng ý của phụ huynh
        [HttpGet("{eventId}/consent-statistics")]
        public async Task<IActionResult> GetConsentStatistics(int eventId)
        {
            var consents = await _context.ParentalConsents
                .Where(c => c.VaccinationEventID == eventId) // Sửa tên trường
                .ToListAsync();

            var statistics = new
            {
                TotalStudents = consents.Count,
                Approved = consents.Count(c => c.ConsentStatus == "Đồng ý"),     // Sửa tên trường
                Rejected = consents.Count(c => c.ConsentStatus == "Từ chối"),    // Sửa tên trường
                Pending = consents.Count(c => c.ConsentStatus == "Chờ phản hồi") // Sửa tên trường
            };

            return Ok(statistics);
        }
    }
}