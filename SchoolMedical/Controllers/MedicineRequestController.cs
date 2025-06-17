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
                    .Where(d => d.RequestID == m.RequestID)
                    .Select(d => new MedicineRequestDetailDTO
                    {
                        RequestDetailID = d.RequestDetailID,
                        RequestID = d.RequestID,
                        ItemID = d.ItemID,
                        Quantity = d.Quantity,
                        DosageInstructions = d.DosageInstructions
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
                    .Where(d => d.RequestID == m.RequestID)
                    .Select(d => new MedicineRequestDetailDTO
                    {
                        RequestDetailID = d.RequestDetailID,
                        RequestID = d.RequestID,
                        ItemID = d.ItemID,
                        Quantity = d.Quantity,
                        DosageInstructions = d.DosageInstructions
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
                    ItemID = detail.ItemID,
                    Quantity = detail.Quantity,
                    DosageInstructions = detail.DosageInstructions
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
}