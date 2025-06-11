using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolMedical.Core.DTOs.MedicineRequest;
using SchoolMedical.Core.Entities;
using SchoolMedical.Infrastructure.Data;

namespace SchoolMedical.API.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class MedicineRequestController : ControllerBase
	{
		private readonly ApplicationDbContext _context;

		public MedicineRequestController(ApplicationDbContext context)
		{
			_context = context;
		}

		// GET: api/MedicineRequest
		[HttpGet]
		public async Task<ActionResult<IEnumerable<MedicineRequestDTO>>> GetMedicineRequests()
		{
			var requests = await _context.MedicineRequests
				.Include(m => m.Student)
				.Include(m => m.Parent)
				.Select(m => new MedicineRequestDTO
				{
					RequestID = m.RequestID,
					Date = m.Date,
					MedicineName = m.MedicineName,
					RequestStatus = m.RequestStatus,
					StudentID = m.StudentID,
					ParentID = m.ParentID,
					AllergenCheck = m.AllergenCheck,
					ApprovedBy = m.ApprovedBy,
					ApprovalDate = m.ApprovalDate,
					StudentName = m.Student.FullName,
					ParentName = m.Parent.FullName
				})
				.ToListAsync();

			return requests;
		}

		// GET: api/MedicineRequest/5
		[HttpGet("{id}")]
		public async Task<ActionResult<MedicineRequestDTO>> GetMedicineRequest(int id)
		{
			var request = await _context.MedicineRequests
				.Include(m => m.Student)
				.Include(m => m.Parent)
				.FirstOrDefaultAsync(m => m.RequestID == id);

			if (request == null)
				return NotFound();

			var dto = new MedicineRequestDTO
			{
				RequestID = request.RequestID,
				Date = request.Date,
				MedicineName = request.MedicineName,
				RequestStatus = request.RequestStatus,
				StudentID = request.StudentID,
				ParentID = request.ParentID,
				AllergenCheck = request.AllergenCheck,
				ApprovedBy = request.ApprovedBy,
				ApprovalDate = request.ApprovalDate,
				StudentName = request.Student?.FullName,
				ParentName = request.Parent?.FullName
			};

			return dto;
		}

		// POST: api/MedicineRequest
		[HttpPost]
		public async Task<ActionResult<MedicineRequestDTO>> CreateMedicineRequest(MedicineRequestCreateRequest request)
		{
			var student = await _context.Students.FindAsync(request.StudentID);
			if (student == null)
				return BadRequest("Invalid StudentID");

			var medicineRequest = new MedicineRequest
			{
				Date = DateTime.UtcNow.Date,
				MedicineName = request.MedicineName,
				RequestStatus = "Pending",
				StudentID = request.StudentID,
				ParentID = request.ParentID,
				AllergenCheck = request.AllergenCheck
			};

			_context.MedicineRequests.Add(medicineRequest);
			await _context.SaveChangesAsync();

			return await GetMedicineRequest(medicineRequest.RequestID);
		}

		// PUT: api/MedicineRequest/5/approve
		[HttpPut("{id}/approve")]
		public async Task<IActionResult> ApproveMedicineRequest(int id, [FromQuery] int approvedBy)
		{
			var medicineRequest = await _context.MedicineRequests.FindAsync(id);
			if (medicineRequest == null)
				return NotFound();

			medicineRequest.RequestStatus = "Approved";
			medicineRequest.ApprovedBy = approvedBy;
			medicineRequest.ApprovalDate = DateTime.UtcNow.Date;

			await _context.SaveChangesAsync();
			return NoContent();
		}

		// PUT: api/MedicineRequest/5/reject
		[HttpPut("{id}/reject")]
		public async Task<IActionResult> RejectMedicineRequest(int id)
		{
			var medicineRequest = await _context.MedicineRequests.FindAsync(id);
			if (medicineRequest == null)
				return NotFound();

			medicineRequest.RequestStatus = "Rejected";
			await _context.SaveChangesAsync();
			return NoContent();
		}

		// GET: api/MedicineRequest/student/5
		[HttpGet("student/{studentId}")]
		public async Task<ActionResult<IEnumerable<MedicineRequestDTO>>> GetStudentMedicineRequests(int studentId)
		{
			var requests = await _context.MedicineRequests
				.Include(m => m.Student)
				.Include(m => m.Parent)
				.Where(m => m.StudentID == studentId)
				.Select(m => new MedicineRequestDTO
				{
					RequestID = m.RequestID,
					Date = m.Date,
					MedicineName = m.MedicineName,
					RequestStatus = m.RequestStatus,
					StudentID = m.StudentID,
					ParentID = m.ParentID,
					AllergenCheck = m.AllergenCheck,
					ApprovedBy = m.ApprovedBy,
					ApprovalDate = m.ApprovalDate,
					StudentName = m.Student.FullName,
					ParentName = m.Parent.FullName
				})
				.ToListAsync();

			return requests;
		}
	}
}