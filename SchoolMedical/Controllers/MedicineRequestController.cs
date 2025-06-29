using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolMedical.Core.Entities;
using SchoolMedical.Infrastructure.Data;
using SchoolMedical.Core.DTOs.MedicineRequest;
using SchoolMedical.Infrastructure.Services;

[ApiController]
[Route("api/[controller]")]
public class MedicineRequestController : ControllerBase
{
	private readonly ApplicationDbContext _context;
	private readonly INotificationService _notificationService;

	public MedicineRequestController(ApplicationDbContext context, INotificationService notificationService)
	{
		_context = context;
		_notificationService = notificationService;
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
						RequestItemID = d.RequestItemID,
						RequestItemName = d.RequestItem.RequestItemName,
						Description = d.RequestItem.Description,
						Quantity = d.Quantity,
						DosageInstructions = d.DosageInstructions,
						Time = d.Time
					}).ToList(),
				NurseNote = m.NurseNote
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
						RequestItemID = d.RequestItemID,
						RequestItemName = d.RequestItem.RequestItemName,
						Description = d.RequestItem.Description,
						Quantity = d.Quantity,
						DosageInstructions = d.DosageInstructions,
						Time = d.Time
					}).ToList(),
				NurseNote = m.NurseNote
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
						RequestItemID = d.RequestItemID,
						RequestItemName = d.RequestItem.RequestItemName,
						Description = d.RequestItem.Description,
						Quantity = d.Quantity,
						DosageInstructions = d.DosageInstructions,
						Time = d.Time
					}).ToList(),
				NurseNote = m.NurseNote
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
						RequestItemID = d.RequestItemID,
						RequestItemName = d.RequestItem.RequestItemName,
						Description = d.RequestItem.Description,
						Quantity = d.Quantity,
						DosageInstructions = d.DosageInstructions,
						Time = d.Time
					}).ToList(),
				NurseNote = m.NurseNote
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
			// Capture the real-time timestamp
			var realTimeDate = DateTime.UtcNow;
			
			// Create main request
			var medicineRequest = new MedicineRequest
			{
				Date = realTimeDate,
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
					Quantity = detail.Quantity,
					DosageInstructions = detail.DosageInstructions,
					Time = detail.Time
				};
				_context.MedicineRequestDetails.Add(requestDetail);
			}

			await _context.SaveChangesAsync();
			await transaction.CommitAsync();

			// Return the created request with real-time timestamp instead of fetching from database
			return await CreateMedicineRequestDTO(medicineRequest.RequestID, realTimeDate);
		}
		catch (Exception)
		{
			await transaction.RollbackAsync();
			throw;
		}
	}

	// PUT: api/MedicineRequest/{id}/approve
	[HttpPut("{id}/approve")]
	public async Task<ActionResult<MedicineRequestDTO>> ApproveMedicineRequest(int id, [FromBody] ApproveRequestDto request)
	{
		var medicineRequest = await _context.MedicineRequests.FindAsync(id);
		if (medicineRequest == null)
			return NotFound();

		// Capture the real-time approval timestamp
		var realTimeApprovalDate = DateTime.UtcNow;

		medicineRequest.RequestStatus = "Approved";
		medicineRequest.ApprovedBy = request.ApprovedBy;
		medicineRequest.ApprovalDate = realTimeApprovalDate;
		medicineRequest.NurseNote = request.NurseNote;

		await _context.SaveChangesAsync();
		
		// Send automatic notification to parent
		await _notificationService.SendMedicineRequestNotificationAsync(id, "Approved", request.NurseNote);
		
		// Return the updated request with real-time approval timestamp
		var updatedRequest = await _context.MedicineRequests
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
				ApprovalDate = realTimeApprovalDate, // Use the real-time approval timestamp
				StudentName = m.Student.FullName,
				ParentName = m.Parent.FullName,
				MedicineDetails = _context.MedicineRequestDetails
					.Include(d => d.RequestItem)
					.Where(d => d.RequestID == m.RequestID)
					.Select(d => new MedicineRequestDetailDTO
					{
						RequestDetailID = d.RequestDetailID,
						RequestItemID = d.RequestItemID,
						RequestItemName = d.RequestItem.RequestItemName,
						Description = d.RequestItem.Description,
						Quantity = d.Quantity,
						DosageInstructions = d.DosageInstructions,
						Time = d.Time
					}).ToList(),
				NurseNote = m.NurseNote
			})
			.FirstOrDefaultAsync();

		return updatedRequest;
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
		medicineRequest.NurseNote = request.NurseNote;
		await _context.SaveChangesAsync();
		
		// Send automatic notification to parent
		await _notificationService.SendMedicineRequestNotificationAsync(id, "Refused", request.NurseNote);
		
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

	// Helper method to create MedicineRequestDTO
	private async Task<MedicineRequestDTO> CreateMedicineRequestDTO(int requestId, DateTime? overrideDate = null, DateTime? overrideApprovalDate = null)
	{
		return await _context.MedicineRequests
			.Include(m => m.Student)
			.Include(m => m.Parent)
			.Where(m => m.RequestID == requestId)
			.Select(m => new MedicineRequestDTO
			{
				RequestID = m.RequestID,
				Date = overrideDate ?? m.Date,
				RequestStatus = m.RequestStatus,
				StudentID = m.StudentID,
				ParentID = m.ParentID,
				Note = m.Note,
				ApprovedBy = m.ApprovedBy,
				ApprovalDate = overrideApprovalDate ?? m.ApprovalDate,
				StudentName = m.Student.FullName,
				ParentName = m.Parent.FullName,
				MedicineDetails = _context.MedicineRequestDetails
					.Include(d => d.RequestItem)
					.Where(d => d.RequestID == m.RequestID)
					.Select(d => new MedicineRequestDetailDTO
					{
						RequestDetailID = d.RequestDetailID,
						RequestItemID = d.RequestItemID,
						RequestItemName = d.RequestItem.RequestItemName,
						Description = d.RequestItem.Description,
						Quantity = d.Quantity,
						DosageInstructions = d.DosageInstructions,
						Time = d.Time
					}).ToList(),
				NurseNote = m.NurseNote
			})
			.FirstOrDefaultAsync();
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