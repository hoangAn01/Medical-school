using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolMedical.Core.DTOs;
using SchoolMedical.Infrastructure.Data;

namespace SchoolMedical.API.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class StudentController : ControllerBase
	{
		private readonly ApplicationDbContext _context;

		public StudentController(ApplicationDbContext context)
		{
			_context = context;
		}

		// GET: api/Student/{parentId}
		[HttpGet("{parentId:int?}")]
		public async Task<ActionResult<IEnumerable<StudentDTO>>> GetStudents(int? parentId)
		{
			var query = _context.Students
				.Include(s => s.Parent)
				.Include(s => s.Class)
				.AsQueryable();

			if (parentId.HasValue)
			{
				// Verify parent exists
				var parentExists = await _context.Parents.AnyAsync(p => p.ParentID == parentId);
				if (!parentExists)
					return NotFound($"Parent with ID {parentId} not found");

				query = query.Where(s => s.ParentID == parentId);
			}

			var students = await query
				.Select(s => new StudentDTO
				{
					StudentID = s.StudentID,
					FullName = s.FullName,
					Gender = s.Gender,
					DateOfBirth = s.DateOfBirth,
					ParentID = s.ParentID,
					UserID = s.UserID,
					ClassID = s.ClassID,
					ParentName = s.Parent != null ? s.Parent.FullName : null,
					ClassName = s.Class != null ? s.Class.ClassName : null
				})
				.ToListAsync();

			return students;
		}

		// GET: api/Student/{studentId}
		[HttpGet("search/{studentId}")]
		public async Task<ActionResult<StudentDTO>> GetStudentInfo(int studentId)
		{
			var student = await _context.Students
				.Include(s => s.Parent)
				.Include(s => s.Class)
				.Where(s => s.StudentID == studentId)
				.Select(s => new StudentDTO
				{
					StudentID = s.StudentID,
					FullName = s.FullName,
					Gender = s.Gender,
					DateOfBirth = s.DateOfBirth,
					ParentID = s.ParentID,
					UserID = s.UserID,
					ClassID = s.ClassID,
					ParentName = s.Parent != null ? s.Parent.FullName : null,
					ClassName = s.Class != null ? s.Class.ClassName : null
				})
				.FirstOrDefaultAsync();

			if (student == null)
				return NotFound($"Student with ID {studentId} not found");

			return student;
		}

		// GET: api/Student/search/{studentName}
		[HttpGet("search/{studentName}")]
		public async Task<ActionResult<IEnumerable<StudentDTO>>> SearchStudentsByName(string studentName)
		{
			if (string.IsNullOrWhiteSpace(studentName))
				return BadRequest("Student name cannot be empty");

			var students = await _context.Students
				.Include(s => s.Parent)
				.Include(s => s.Class)
				.Where(s => s.FullName != null && s.FullName.Contains(studentName))
				.Select(s => new StudentDTO
				{
					StudentID = s.StudentID,
					FullName = s.FullName,
					Gender = s.Gender,
					DateOfBirth = s.DateOfBirth,
					ParentID = s.ParentID,
					UserID = s.UserID,
					ClassID = s.ClassID,
					ParentName = s.Parent != null ? s.Parent.FullName : null,
					ClassName = s.Class != null ? s.Class.ClassName : null
				})
				.ToListAsync();

			if (!students.Any())
				return NotFound($"No students found with name containing '{studentName}'");

			return students;
		}
	}
}