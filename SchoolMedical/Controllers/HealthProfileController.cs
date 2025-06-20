using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolMedical.Core.Entities;
using SchoolMedical.Infrastructure.Data;
using SchoolMedical.Core.DTOs.HealthProfile;

namespace SchoolMedical.API.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class HealthProfileController : ControllerBase
	{
		private readonly ApplicationDbContext _context;

		public HealthProfileController(ApplicationDbContext context)
		{
			_context = context;
		}

		// GET: api/HealthProfile
		[HttpGet]
		public async Task<ActionResult<IEnumerable<HealthProfileDTO>>> GetHealthProfiles()
		{
			var profiles = await _context.HealthProfiles
				.Include(h => h.Student)
				.Select(h => new HealthProfileDTO
				{
					ProfileID = h.ProfileID,
					StudentID = h.StudentID,
					ChronicDisease = h.ChronicDisease,
					VisionTest = h.VisionTest,
					Allergy = h.Allergy,
					Weight = h.Weight,
					Height = h.Height,
					LastCheckupDate = h.LastCheckupDate,
					StudentFullName = h.Student != null ? h.Student.FullName : null
				})
				.ToListAsync();

			return profiles;
		}

		// GET: api/HealthProfile/5
		[HttpGet("{id}")]
		public async Task<ActionResult<HealthProfileDTO>> GetHealthProfile(int id)
		{
			var h = await _context.HealthProfiles
				.Include(hp => hp.Student)
				.FirstOrDefaultAsync(hp => hp.ProfileID == id);

			if (h == null) return NotFound();

			var dto = new HealthProfileDTO
			{
				ProfileID = h.ProfileID,
				StudentID = h.StudentID,
				ChronicDisease = h.ChronicDisease,
				VisionTest = h.VisionTest,
				Allergy = h.Allergy,
				Weight = h.Weight,
				Height = h.Height,
				LastCheckupDate = h.LastCheckupDate,
				StudentFullName = h.Student != null ? h.Student.FullName : null
			};
			return dto;
		}

		// GET: api/HealthProfile/searchByName
		[HttpGet("searchByName")]
		public async Task<ActionResult<IEnumerable<HealthProfileDTO>>> SearchByStudentName([FromQuery] string name)
		{
			if (string.IsNullOrWhiteSpace(name))
				return BadRequest("Search name cannot be empty");

			var profiles = await _context.HealthProfiles
				.Include(h => h.Student)
				.Where(h => h.Student.FullName.Contains(name))
				.Select(h => new HealthProfileDTO
				{
					ProfileID = h.ProfileID,
					StudentID = h.StudentID,
					ChronicDisease = h.ChronicDisease,
					VisionTest = h.VisionTest,
					Allergy = h.Allergy,
					Weight = h.Weight,
					Height = h.Height,
					LastCheckupDate = h.LastCheckupDate,
					StudentFullName = h.Student != null ? h.Student.FullName : null
				})
				.ToListAsync();

			return profiles;
		}

		 // GET: api/HealthProfile/student/{studentId}
		[HttpGet("search/{studentId}")]
		public async Task<ActionResult<IEnumerable<HealthProfileDTO>>> GetHealthProfilesByStudentId(int studentId)
		{
			// Verify student exists
			var student = await _context.Students.FindAsync(studentId);
			if (student == null)
				return NotFound($"Student with ID {studentId} not found");

			var profiles = await _context.HealthProfiles
				.Include(h => h.Student)
				.Where(h => h.StudentID == studentId)
				.Select(h => new HealthProfileDTO
				{
					ProfileID = h.ProfileID,
					StudentID = h.StudentID,
					ChronicDisease = h.ChronicDisease,
					VisionTest = h.VisionTest,
					Allergy = h.Allergy,
					Weight = h.Weight,
					Height = h.Height,
					LastCheckupDate = h.LastCheckupDate,
					StudentFullName = h.Student != null ? h.Student.FullName : null
				})
				.ToListAsync();

			return profiles;
		}

		// POST: api/HealthProfile
		[HttpPost]
		public async Task<ActionResult<HealthProfileDTO>> CreateHealthProfile(HealthProfileRequest request)
		{
			var healthProfile = new HealthProfile
			{
				StudentID = request.StudentID,
				ChronicDisease = request.ChronicDisease,
				VisionTest = request.VisionTest,
				Allergy = request.Allergy,
				Weight = request.Weight,
				Height = request.Height,
				LastCheckupDate = request.LastCheckupDate
			};

			_context.HealthProfiles.Add(healthProfile);
			await _context.SaveChangesAsync();

			// Optionally fetch student for response
			var student = await _context.Students.FindAsync(healthProfile.StudentID);

			var dto = new HealthProfileDTO
			{
				ProfileID = healthProfile.ProfileID,
				StudentID = healthProfile.StudentID,
				ChronicDisease = healthProfile.ChronicDisease,
				VisionTest = healthProfile.VisionTest,
				Allergy = healthProfile.Allergy,
				Weight = healthProfile.Weight,
				Height = healthProfile.Height,
				LastCheckupDate = healthProfile.LastCheckupDate,
				StudentFullName = student?.FullName
			};

			return CreatedAtAction(nameof(GetHealthProfile), new { id = dto.ProfileID }, dto);
		}

		// PUT: api/HealthProfile/5
		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateHealthProfile(int id, HealthProfileRequest request)
		{
			if (request.ProfileID == null || id != request.ProfileID)
				return BadRequest();

			var healthProfile = await _context.HealthProfiles.FindAsync(id);
			if (healthProfile == null)
				return NotFound();

			healthProfile.StudentID = request.StudentID;
			healthProfile.ChronicDisease = request.ChronicDisease;
			healthProfile.VisionTest = request.VisionTest;
			healthProfile.Allergy = request.Allergy;
			healthProfile.Weight = request.Weight;
			healthProfile.Height = request.Height;
			healthProfile.LastCheckupDate = request.LastCheckupDate;

			await _context.SaveChangesAsync();

			return NoContent();
		}

		// DELETE: api/HealthProfile/5
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteHealthProfile(int id)
		{
			var healthProfile = await _context.HealthProfiles.FindAsync(id);
			if (healthProfile == null) return NotFound();

			_context.HealthProfiles.Remove(healthProfile);
			await _context.SaveChangesAsync();

			return NoContent();
		}

		// GET: api/HealthProfile/students-without-profile?parentId=123
		[HttpGet("students-without-profile")]
		public async Task<ActionResult<IEnumerable<StudentInfoDTO>>> GetStudentsWithoutHealthProfile([FromQuery] int parentId)
		{
			// Validate parent exists
			var parent = await _context.Parents.FindAsync(parentId);
			if (parent == null)
				return NotFound($"Parent with ID {parentId} not found");

			var students = await _context.Students
				.Where(s => s.ParentID == parentId && !_context.HealthProfiles.Any(hp => hp.StudentID == s.StudentID))
				.Select(s => new StudentInfoDTO
				{
					StudentID = s.StudentID,
					FullName = s.FullName,
					Gender = s.Gender,
					DateOfBirth = s.DateOfBirth,
					ClassID = s.ClassID,
					ParentID = s.ParentID,
					UserID = s.UserID
				})
				.ToListAsync();

			return Ok(students);
		}
	}
}

public class StudentInfoDTO
{
	public int StudentID { get; set; }
	public string? FullName { get; set; }
	public char? Gender { get; set; }
	public DateTime? DateOfBirth { get; set; }
	public int? ClassID { get; set; }
	public int? ParentID { get; set; }
	public int? UserID { get; set; }
}