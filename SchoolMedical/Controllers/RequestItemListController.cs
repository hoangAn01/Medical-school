using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolMedical.Core.Entities;
using SchoolMedical.Infrastructure.Data;
using SchoolMedical.Core.DTOs.MedicineRequest;

[ApiController]
[Route("api/[controller]")]
public class RequestItemListController : ControllerBase
{
	private readonly ApplicationDbContext _context;

	public RequestItemListController(ApplicationDbContext context)
	{
		_context = context;
	}

	// GET: api/RequestItemList
	[HttpGet]
	public async Task<ActionResult<IEnumerable<RequestItemListDTO>>> GetAllRequestItems()
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

	// GET: api/RequestItemList/{id}
	[HttpGet("{id}")]
	public async Task<ActionResult<RequestItemListDTO>> GetRequestItem(int id)
	{
		var item = await _context.RequestItemList
			.Where(ri => ri.RequestItemID == id)
			.Select(ri => new RequestItemListDTO
			{
				RequestItemID = ri.RequestItemID,
				RequestItemName = ri.RequestItemName,
				Description = ri.Description
			})
			.FirstOrDefaultAsync();

		if (item == null) return NotFound();
		return Ok(item);
	}

	// POST: api/RequestItemList
	[HttpPost]
	public async Task<ActionResult<RequestItemListDTO>> CreateRequestItem([FromBody] RequestItemListDTO request)
	{
		var newItem = new RequestItemList
		{
			RequestItemName = request.RequestItemName,
			Description = request.Description
		};

		_context.RequestItemList.Add(newItem);
		await _context.SaveChangesAsync();

		request.RequestItemID = newItem.RequestItemID;
		return CreatedAtAction(nameof(GetRequestItem), new { id = newItem.RequestItemID }, request);
	}

	// PUT: api/RequestItemList/{id}
	[HttpPut("{id}")]
	public async Task<IActionResult> UpdateRequestItem(int id, [FromBody] RequestItemListDTO request)
	{
		var item = await _context.RequestItemList.FindAsync(id);
		if (item == null) return NotFound();

		item.RequestItemName = request.RequestItemName;
		item.Description = request.Description;

		await _context.SaveChangesAsync();
		return NoContent();
	}

	// DELETE: api/RequestItemList/{id}
	[HttpDelete("{id}")]
	public async Task<IActionResult> DeleteRequestItem(int id)
	{
		var item = await _context.RequestItemList.FindAsync(id);
		if (item == null) return NotFound();

		// Check if the item is being used in any medicine requests
		var isUsed = await _context.MedicineRequestDetails
			.AnyAsync(d => d.RequestItemID == id);

		if (isUsed)
			return BadRequest("Cannot delete item as it is being used in medicine requests");

		_context.RequestItemList.Remove(item);
		await _context.SaveChangesAsync();
		return NoContent();
	}
} 