using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolMedical.Core.Entities;
using SchoolMedical.Infrastructure.Data;
using SchoolMedical.Core.DTOs.MedicineRequest;

[ApiController]
[Route("api/[controller]")]
public class MedicineRequestController : ControllerBase
{
	private readonly ApplicationDbContext _context;

	public MedicineRequestController(ApplicationDbContext context)
	{
		_context = context;
	}

	// GET: api/MedicineRequest/{id}
	[HttpGet("{id}")]
	public async Task<ActionResult<MedicineRequestDTO>> GetMedicineRequest(int id)
	{
		var dto = await _context.MedicineRequests
			.Include(m => m.Student)
			.Include(m => m.Parent)
			.Where(m => m.RequestID == id)
			.Select(m => new MedicineRequestDTO
			{
				RequestID = m.RequestID,
				Date = m.Date,
				RequestStatus = m.RequestStatus,
				StudentID = m.StudentID,
				ParentID = m.ParentID,
				Note = m.Note,
				ApprovedBy = m.ApprovedBy,
				ApprovalDate = m.ApprovalDate,
				StudentName = m.Student.FullName,
				ParentName = m.Parent.FullName,
				MedicineDetails = _context.MedicineRequestDetails
					.Include(d => d.RequestItem)
					.Where(d => d.RequestID == m.RequestID)
					.Select(d => new MedicineRequestDetailDTO
					{
						RequestDetailID = d.RequestDetailID,
						RequestID = d.RequestID,
						RequestItemID = d.RequestItemID,
						RequestItemName = d.RequestItem.RequestItemName,
						Description = d.RequestItem.Description,
						// MedicineType = d.MedicineType, // commented out
						Quantity = d.Quantity,
						DosageInstructions = d.DosageInstructions,
						Time = d.Time
					}).ToList()
			})
			.FirstOrDefaultAsync();

		if (dto == null) return NotFound();
		return dto;
	}

	// GET: api/MedicineRequest/getAll
	[HttpGet("getAll")]
	public async Task<ActionResult<IEnumerable<MedicineRequestDTO>>> GetAllMedicineRequests()
	{
		var requests = await _context.MedicineRequests
			.Include(m => m.Student)
			.Include(m => m.Parent)
			.Select(m => new MedicineRequestDTO
			{
				RequestID = m.RequestID,
				Date = m.Date,
				RequestStatus = m.RequestStatus,
				StudentID = m.StudentID,
				ParentID = m.ParentID,
				Note = m.Note,
				ApprovedBy = m.ApprovedBy,
				ApprovalDate = m.ApprovalDate,
				StudentName = m.Student.FullName,
				ParentName = m.Parent.FullName,
				MedicineDetails = _context.MedicineRequestDetails
					.Include(d => d.RequestItem)
					.Where(d => d.RequestID == m.RequestID)
					.Select(d => new MedicineRequestDetailDTO
					{
						RequestDetailID = d.RequestDetailID,
						RequestID = d.RequestID,
						RequestItemID = d.RequestItemID,
						RequestItemName = d.RequestItem.RequestItemName,
						Description = d.RequestItem.Description,
						// MedicineType = d.MedicineType, // commented out
						Quantity = d.Quantity,
						DosageInstructions = d.DosageInstructions,
						Time = d.Time
					}).ToList()
			})
			.OrderByDescending(m => m.Date)
			.ToListAsync();
		return Ok(requests);
	}

	// GET: api/MedicineRequest/sent/{parentId}
	[HttpGet("sent/{parentId}")]
	public async Task<ActionResult<IEnumerable<MedicineRequestDTO>>> GetSentMedicineRequests([FromRoute] int parentId)
	{
		// Validate parent exists
		var parent = await _context.Parents.FindAsync(parentId);
		if (parent == null)
			return BadRequest("Invalid ParentID");

		var requests = await _context.MedicineRequests
			.Include(m => m.Student)
			.Include(m => m.Parent)
			.Where(m => m.ParentID == parentId)
			.Select(m => new MedicineRequestDTO
			{
				RequestID = m.RequestID,
				Date = m.Date,
				RequestStatus = m.RequestStatus,
				StudentID = m.StudentID,
				ParentID = m.ParentID,
				Note = m.Note,
				ApprovedBy = m.ApprovedBy,
				ApprovalDate = m.ApprovalDate,
				StudentName = m.Student.FullName,
				ParentName = m.Parent.FullName,
				MedicineDetails = _context.MedicineRequestDetails
					.Include(d => d.RequestItem)
					.Where(d => d.RequestID == m.RequestID)
					.Select(d => new MedicineRequestDetailDTO
					{
						RequestDetailID = d.RequestDetailID,
						RequestID = d.RequestID,
						RequestItemID = d.RequestItemID,
						RequestItemName = d.RequestItem.RequestItemName,
						Description = d.RequestItem.Description,
						// MedicineType = d.MedicineType, // commented out
						Quantity = d.Quantity,
						DosageInstructions = d.DosageInstructions,
						Time = d.Time
					}).ToList()
			})
			.OrderByDescending(m => m.Date)
			.ToListAsync();

		return Ok(requests);
	}

	// GET: api/MedicineRequest/pending
	[HttpGet("pending")]
	public async Task<ActionResult<IEnumerable<MedicineRequestDTO>>> GetPendingMedicineRequests()
	{
		var requests = await _context.MedicineRequests
			.Include(m => m.Student)
			.Include(m => m.Parent)
			.Where(m => m.RequestStatus == "Pending")
			.Select(m => new MedicineRequestDTO
			{
				RequestID = m.RequestID,
				Date = m.Date,
				RequestStatus = m.RequestStatus,
				StudentID = m.StudentID,
				ParentID = m.ParentID,
				Note = m.Note,
				ApprovedBy = m.ApprovedBy,
				ApprovalDate = m.ApprovalDate,
				StudentName = m.Student.FullName,
				ParentName = m.Parent.FullName,
				MedicineDetails = _context.MedicineRequestDetails
					.Include(d => d.RequestItem)
					.Where(d => d.RequestID == m.RequestID)
					.Select(d => new MedicineRequestDetailDTO
					{
						RequestDetailID = d.RequestDetailID,
						RequestID = d.RequestID,
						RequestItemID = d.RequestItemID,
						RequestItemName = d.RequestItem.RequestItemName,
						Description = d.RequestItem.Description,
						// MedicineType = d.MedicineType, // commented out
						Quantity = d.Quantity,
						DosageInstructions = d.DosageInstructions,
						Time = d.Time
					}).ToList()
			})
			.OrderByDescending(m => m.Date)
			.ToListAsync();

		return Ok(requests);
	}

	// POST: api/MedicineRequest
	[HttpPost]
	public async Task<ActionResult<MedicineRequestDTO>> CreateMedicineRequest(MedicineRequestCreateRequest request)
	{
		// Validate student exists
		var student = await _context.Students.FindAsync(request.StudentID);
		if (student == null)
			return BadRequest("Invalid StudentID");

		// Validate all RequestItemIDs exist
		var requestItemIds = request.MedicineDetails.Select(d => d.RequestItemID).Distinct().ToList();
		var existingItems = await _context.RequestItemList
			.Where(ri => requestItemIds.Contains(ri.RequestItemID))
			.Select(ri => ri.RequestItemID)
			.ToListAsync();

		var missingItems = requestItemIds.Except(existingItems).ToList();
		if (missingItems.Any())
			return BadRequest($"Invalid RequestItemID(s): {string.Join(", ", missingItems)}");

		using var transaction = await _context.Database.BeginTransactionAsync();
		try
		{
			// Create main request
			var medicineRequest = new MedicineRequest
			{
				Date = DateTime.UtcNow,
				RequestStatus = "Pending",
				StudentID = request.StudentID,
				ParentID = request.ParentID,
				Note = request.Note
			};

			_context.MedicineRequests.Add(medicineRequest);
			await _context.SaveChangesAsync();

			// Add request details
			foreach (var detail in request.MedicineDetails)
			{
				var requestDetail = new MedicineRequestDetail
				{
					RequestID = medicineRequest.RequestID,
					RequestItemID = detail.RequestItemID,
					// MedicineType = detail.MedicineType, // commented out
					Quantity = detail.Quantity,
					DosageInstructions = detail.DosageInstructions,
					Time = detail.Time
				};
				_context.MedicineRequestDetails.Add(requestDetail);
			}

			await _context.SaveChangesAsync();
			await transaction.CommitAsync();

			// Return the created request with details
			return await GetMedicineRequest(medicineRequest.RequestID);
		}
		catch (Exception)
		{
			await transaction.RollbackAsync();
			throw;
		}
	}

	// PUT: api/MedicineRequest/{id}/approve
	[HttpPut("{id}/approve")]
	public async Task<IActionResult> ApproveMedicineRequest(int id, [FromBody] ApproveRequestDto request)
	{
		var medicineRequest = await _context.MedicineRequests.FindAsync(id);
		if (medicineRequest == null)
			return NotFound();

		medicineRequest.RequestStatus = "Approved";
		medicineRequest.ApprovedBy = request.ApprovedBy;
		medicineRequest.ApprovalDate = DateTime.UtcNow.Date;

		// Append nurse note to existing note (with separator if needed)
		if (!string.IsNullOrWhiteSpace(request.NurseNote))
		{
			if (!string.IsNullOrWhiteSpace(medicineRequest.Note))
				medicineRequest.Note += "\n---\n";
			medicineRequest.Note += $"Nurse note: {request.NurseNote}";
		}

		await _context.SaveChangesAsync();
		return NoContent();
	}

	// PUT: api/MedicineRequest/{id}/refuse
	[HttpPut("{id}/refuse")]
	public async Task<IActionResult> RefuseMedicineRequest(int id, [FromBody] RefuseRequestDto request)
	{
		var medicineRequest = await _context.MedicineRequests.FindAsync(id);
		if (medicineRequest == null)
			return NotFound();

		// Set status to "Refused"
		medicineRequest.RequestStatus = "Refused";

		// Append nurse note to existing note (with separator if needed)
		if (!string.IsNullOrWhiteSpace(request.NurseNote))
		{
			if (!string.IsNullOrWhiteSpace(medicineRequest.Note))
				medicineRequest.Note += "\n---\n";
			medicineRequest.Note += $"Nurse note: {request.NurseNote}";
		}
		await _context.SaveChangesAsync();
		return NoContent();
	}

	// GET: api/MedicineRequest/items
	[HttpGet("items")]
	public async Task<ActionResult<IEnumerable<RequestItemListDTO>>> GetAllItemNamesAndMedicineTypes()
	{
		var items = await _context.RequestItemList
			.Select(ri => new RequestItemListDTO
			{
				RequestItemID = ri.RequestItemID,
				RequestItemName = ri.RequestItemName,
				Description = ri.Description
			})
			.ToListAsync();

		return Ok(items);
	}
}

public class RefuseRequestDto
{
	public string NurseNote { get; set; } = string.Empty;
}

public class ApproveRequestDto
{
	public int ApprovedBy { get; set; }
	public string NurseNote { get; set; } = string.Empty;
}