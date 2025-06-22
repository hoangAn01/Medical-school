using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolMedical.Core.DTOs;
using SchoolMedical.Core.DTOs.ParentalConsent;
using SchoolMedical.Core.Entities;
using SchoolMedical.Core.Interfaces.Services;
using SchoolMedical.Infrastructure.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;
using SchoolMedical.Core.DTOs.SchoolCheckup;

namespace SchoolMedical.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CheckupController : ControllerBase
    {
        private readonly ICheckupService _checkupService;
        private readonly ApplicationDbContext _context;

        public CheckupController(ICheckupService checkupService, ApplicationDbContext context)
        {
            _checkupService = checkupService;
            _context = context;
        }

        // GET: api/Checkup/reports
        [HttpGet("reports")]
        public async Task<ActionResult<IEnumerable<HealthReportDTO>>> GetHealthReports()
        {
            var reports = await _context.HealthReport
                .Include(hr => hr.Student)
                .Include(hr => hr.Nurse)
                .Select(hr => new HealthReportDTO
                {
                    ReportID = hr.ReportID,
                    Date = hr.Date,
                    Description = hr.Description,
                    StudentID = hr.StudentID,
                    StudentName = hr.Student.FullName,
                    NurseID = hr.NurseID,
                    NurseName = hr.Nurse != null ? hr.Nurse.FullName : null,
                    HasCheckup = hr.SchoolCheckup != null
                })
                .OrderByDescending(hr => hr.Date)
                .ToListAsync();

            return Ok(reports);
        }

        // GET: api/Checkup/reports/student/{studentId}
        [HttpGet("reports/student/{studentId}")]
        public async Task<ActionResult<IEnumerable<HealthReportDTO>>> GetHealthReportsByStudent(int studentId)
        {
            var reports = await _context.HealthReport
                .Include(hr => hr.Student)
                .Include(hr => hr.Nurse)
                .Where(hr => hr.StudentID == studentId)
                .Select(hr => new HealthReportDTO
                {
                    ReportID = hr.ReportID,
                    Date = hr.Date,
                    Description = hr.Description,
                    StudentID = hr.StudentID,
                    StudentName = hr.Student.FullName,
                    NurseID = hr.NurseID,
                    NurseName = hr.Nurse != null ? hr.Nurse.FullName : null,
                    HasCheckup = hr.SchoolCheckup != null
                })
                .OrderByDescending(hr => hr.Date)
                .ToListAsync();

            return Ok(reports);
        }

        // POST: api/Checkup/reports
        [HttpPost("reports")]
        public async Task<ActionResult<HealthReportDTO>> CreateHealthReport(CreateHealthReportRequest request)
        {
            // Kiểm tra xem student có tồn tại không
            var student = await _context.Students.FindAsync(request.StudentID);
            if (student == null)
                return BadRequest("Học sinh không tồn tại");

            var healthReport = new HealthReport
            {
                Date = request.Date ?? DateTime.Now,
                Description = request.Description,
                StudentID = request.StudentID,
                NurseID = request.NurseID
            };

            _context.HealthReport.Add(healthReport);
            await _context.SaveChangesAsync();

            // Lấy thông tin đầy đủ của report vừa tạo
            var createdReport = await _context.HealthReport
                .Include(hr => hr.Student)
                .Include(hr => hr.Nurse)
                .Where(hr => hr.ReportID == healthReport.ReportID)
                .Select(hr => new HealthReportDTO
                {
                    ReportID = hr.ReportID,
                    Date = hr.Date,
                    Description = hr.Description,
                    StudentID = hr.StudentID,
                    StudentName = hr.Student.FullName,
                    NurseID = hr.NurseID,
                    NurseName = hr.Nurse != null ? hr.Nurse.FullName : null,
                    HasCheckup = false
                })
                .FirstOrDefaultAsync();

            return CreatedAtAction(nameof(GetHealthReportById), new { id = createdReport.ReportID }, createdReport);
        }

        // GET: api/Checkup/reports/{id}
        [HttpGet("reports/{id}")]
        public async Task<ActionResult<HealthReportDTO>> GetHealthReportById(int id)
        {
            var report = await _context.HealthReport
                .Include(hr => hr.Student)
                .Include(hr => hr.Nurse)
                .Where(hr => hr.ReportID == id)
                .Select(hr => new HealthReportDTO
                {
                    ReportID = hr.ReportID,
                    Date = hr.Date,
                    Description = hr.Description,
                    StudentID = hr.StudentID,
                    StudentName = hr.Student.FullName,
                    NurseID = hr.NurseID,
                    NurseName = hr.Nurse != null ? hr.Nurse.FullName : null,
                    HasCheckup = hr.SchoolCheckup != null
                })
                .FirstOrDefaultAsync();

            if (report == null)
                return NotFound();

            return Ok(report);
        }

        [HttpGet]
        public async Task<ActionResult<List<SchoolCheckupDTO>>> GetAllCheckups()
        {
            var checkups = await _checkupService.GetAllCheckupsAsync();
            return Ok(checkups);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SchoolCheckupDTO>> GetCheckupById(int id)
        {
            var checkup = await _checkupService.GetCheckupByIdAsync(id);
            if (checkup == null) return NotFound();
            return Ok(checkup);
        }

        [HttpGet("student/{studentId}")]
        public async Task<ActionResult<List<SchoolCheckupDTO>>> GetCheckupsByStudentId(int studentId)
        {
            var checkups = await _checkupService.GetCheckupsByStudentIdAsync(studentId);
            return Ok(checkups);
        }

        [HttpPost]
        public async Task<ActionResult<SchoolCheckupDTO>> CreateCheckup(CreateSchoolCheckupRequest request)
        {
            // Kiểm tra xem reportID có tồn tại không
            var healthReport = await _context.HealthReport.FindAsync(request.ReportID);
            if (healthReport == null)
                return BadRequest("Health Report không tồn tại. Vui lòng tạo Health Report trước bằng cách gọi POST /api/checkup/reports");

            // Kiểm tra xem đã có checkup cho report này chưa
            var existingCheckup = await _context.SchoolCheckup
                .FirstOrDefaultAsync(sc => sc.ReportID == request.ReportID);
            
            if (existingCheckup != null)
                return BadRequest("Health Report này đã có kết quả khám. Vui lòng sử dụng PUT để cập nhật.");

            var checkup = await _checkupService.CreateCheckupAsync(request);
            
            // Tự động cập nhật HealthProfile
            await UpdateHealthProfileFromCheckupInternal(checkup.CheckupID);
            
            return CreatedAtAction(nameof(GetCheckupById), new { id = checkup.CheckupID }, checkup);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCheckup(int id, CreateSchoolCheckupRequest request)
        {
            var result = await _checkupService.UpdateCheckupAsync(id, request);
            if (!result) return NotFound();
            
            // Tự động cập nhật HealthProfile
            await UpdateHealthProfileFromCheckupInternal(id);
            
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCheckup(int id)
        {
            var result = await _checkupService.DeleteCheckupAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpGet("detail/{checkupId}/parent/{parentId}")]
        public async Task<ActionResult<SchoolCheckupDetailDTO>> GetCheckupDetailForParent(int checkupId, int parentId)
        {
            // Kiểm tra quyền truy cập
            var checkup = await _context.SchoolCheckup
                .Include(sc => sc.HealthReport)
                .ThenInclude(hr => hr.Student)
                .FirstOrDefaultAsync(sc => sc.CheckupID == checkupId);

            if (checkup == null)
                return NotFound();

            // Kiểm tra xem phụ huynh có phải là phụ huynh của học sinh không
            var student = checkup.HealthReport.Student;
            if (student.ParentID != parentId)
                return Forbid();

            // Đánh dấu thông báo đã đọc
            var notification = await _context.Notifications
                .FirstOrDefaultAsync(n => n.CheckupID == checkupId);
            
            if (notification != null)
            {
                var parentNotification = await _context.ParentNotifications
                    .FirstOrDefaultAsync(pn => pn.NotificationID == notification.NotificationID && pn.ParentID == parentId);
                
                if (parentNotification != null && parentNotification.IndividualStatus == "Unread")
                {
                    parentNotification.IndividualStatus = "Read";
                    await _context.SaveChangesAsync();
                }
            }

            // Trả về chi tiết kết quả khám
            var result = new SchoolCheckupDetailDTO
            {
                CheckupID = checkup.CheckupID,
                Date = checkup.HealthReport.Date,
                StudentName = student.FullName,
                Weight = checkup.Weight,
                Height = checkup.Height,
                BloodPressure = checkup.BloodPressure,
                VisionLeft = checkup.VisionLeft,
                VisionRight = checkup.VisionRight,
                Description = checkup.HealthReport.Description,
                NurseName = checkup.HealthReport.Nurse?.FullName
            };

            return Ok(result);
        }
        
        // Endpoint mới cho phụ huynh phản hồi về kết quả khám sử dụng ParentalConsent
        [HttpPost("response/{checkupId}/parent/{parentId}")]
        public async Task<IActionResult> RespondToCheckup(int checkupId, int parentId, [FromBody] CheckupResponseRequest request)
        {
            // Kiểm tra quyền truy cập
            var checkup = await _context.SchoolCheckup
                .Include(sc => sc.HealthReport)
                .ThenInclude(hr => hr.Student)
                .FirstOrDefaultAsync(sc => sc.CheckupID == checkupId);

            if (checkup == null)
                return NotFound("Không tìm thấy kết quả khám");

            // Kiểm tra xem phụ huynh có phải là phụ huynh của học sinh không
            var student = checkup.HealthReport.Student;
            if (student.ParentID != parentId)
                return Forbid();

            // Kiểm tra trạng thái phản hồi hợp lệ
            if (request.ResponseStatus != "Đã đồng ý" && 
                request.ResponseStatus != "Từ chối" && 
                request.ResponseStatus != "Chờ phản hồi")
            {
                return BadRequest("Trạng thái phản hồi không hợp lệ. Các giá trị hợp lệ: 'Đã đồng ý', 'Từ chối', 'Chờ phản hồi'");
            }

            // Tìm hoặc tạo mới bản ghi ParentalConsent
            var consent = await _context.ParentalConsents
                .FirstOrDefaultAsync(pc => pc.CheckupID == checkupId && pc.ParentID == parentId);

            if (consent == null)
            {
                // Tạo mới nếu chưa có
                consent = new ParentalConsent
                {
                    StudentID = student.StudentID,
                    CheckupID = checkupId,
                    ParentID = parentId,
                    ConsentStatus = request.ResponseStatus,
                    ConsentDate = DateTime.Now,
                    Note = request.Note
                };
                _context.ParentalConsents.Add(consent);
            }
            else
            {
                // Cập nhật nếu đã có
                consent.ConsentStatus = request.ResponseStatus;
                consent.ConsentDate = DateTime.Now;
                consent.Note = request.Note;
            }

            await _context.SaveChangesAsync();

            return Ok(new { Message = "Phản hồi đã được ghi nhận", Status = consent.ConsentStatus });
        }

        // Thêm endpoint mới để cập nhật HealthProfile từ kết quả khám
        [HttpPost("update-health-profile/{checkupId}")]
        public async Task<IActionResult> UpdateHealthProfileFromCheckup(int checkupId)
        {
            // Lấy thông tin checkup
            var checkup = await _context.SchoolCheckup
                .Include(sc => sc.HealthReport)
                .ThenInclude(hr => hr.Student)
                .FirstOrDefaultAsync(sc => sc.CheckupID == checkupId);

            if (checkup == null)
                return NotFound("Không tìm thấy kết quả khám");

            var student = checkup.HealthReport.Student;
            if (student == null)
                return NotFound("Không tìm thấy thông tin học sinh");

            // Tìm hoặc tạo mới HealthProfile
            var healthProfile = await _context.HealthProfiles.FirstOrDefaultAsync(hp => hp.StudentID == student.StudentID);
            
            if (healthProfile == null)
            {
                // Tạo mới nếu chưa có
                healthProfile = new HealthProfile
                {
                    StudentID = student.StudentID,
                    Weight = checkup.Weight,
                    Height = checkup.Height,
                    VisionTest = $"{checkup.VisionLeft}/{checkup.VisionRight}",
                    LastCheckupDate = checkup.HealthReport.Date
                };
                _context.HealthProfiles.Add(healthProfile);
            }
            else
            {
                // Cập nhật nếu đã có
                healthProfile.Weight = checkup.Weight;
                healthProfile.Height = checkup.Height;
                healthProfile.VisionTest = $"{checkup.VisionLeft}/{checkup.VisionRight}";
                healthProfile.LastCheckupDate = checkup.HealthReport.Date;
            }

            await _context.SaveChangesAsync();

            return Ok(new { 
                Message = "Đã cập nhật hồ sơ sức khỏe từ kết quả khám",
                ProfileID = healthProfile.ProfileID
            });
        }

        // Phương thức nội bộ để cập nhật HealthProfile
        private async Task<bool> UpdateHealthProfileFromCheckupInternal(int checkupId)
        {
            // Lấy thông tin checkup
            var checkup = await _context.SchoolCheckup
                .Include(sc => sc.HealthReport)
                .ThenInclude(hr => hr.Student)
                .FirstOrDefaultAsync(sc => sc.CheckupID == checkupId);

            if (checkup == null)
                return false;

            var student = checkup.HealthReport.Student;
            if (student == null)
                return false;

            // Tìm hoặc tạo mới HealthProfile
            var healthProfile = await _context.HealthProfiles.FirstOrDefaultAsync(hp => hp.StudentID == student.StudentID);
            
            if (healthProfile == null)
            {
                // Tạo mới nếu chưa có
                healthProfile = new HealthProfile
                {
                    StudentID = student.StudentID,
                    Weight = checkup.Weight,
                    Height = checkup.Height,
                    VisionTest = $"{checkup.VisionLeft}/{checkup.VisionRight}",
                    LastCheckupDate = checkup.HealthReport.Date
                };
                _context.HealthProfiles.Add(healthProfile);
            }
            else
            {
                // Cập nhật nếu đã có
                healthProfile.Weight = checkup.Weight;
                healthProfile.Height = checkup.Height;
                healthProfile.VisionTest = $"{checkup.VisionLeft}/{checkup.VisionRight}";
                healthProfile.LastCheckupDate = checkup.HealthReport.Date;
            }

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
