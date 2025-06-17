using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolMedical.Core.DTOs.MedicalInventory;
using SchoolMedical.Core.Entities;
using SchoolMedical.Infrastructure.Data;
namespace SchoolMedical.API.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class MedicalInventoryController : ControllerBase
	{
		private readonly ApplicationDbContext _context;

		public MedicalInventoryController(ApplicationDbContext context)
		{
			_context = context;
		}   

		// GET: api/MedicalInventory
		[HttpGet]
		public async Task<ActionResult<IEnumerable<MedicalInventoryDTO>>> GetInventory()
		{
			var items = await _context.MedicalInventory
				.Select(i => new MedicalInventoryDTO
				{
					ItemID = i.ItemID,
					ItemName = i.ItemName,
					Category = i.Category,
					Quantity = i.Quantity,
					Unit = i.Unit,
					Description = i.Description
				})
				.ToListAsync();
			return items;
		}

		// GET: api/MedicalInventory/5
		[HttpGet("{id}")]
		public async Task<ActionResult<MedicalInventoryDTO>> GetInventoryItem(int id)
		{
			var item = await _context.MedicalInventory
				.Where(i => i.ItemID == id)
				.Select(i => new MedicalInventoryDTO
				{
					ItemID = i.ItemID,
					ItemName = i.ItemName,
					Category = i.Category,
					Quantity = i.Quantity,
					Unit = i.Unit,
					Description = i.Description
				})
				.FirstOrDefaultAsync();

			if (item == null)
				return NotFound();

			return item;
		}

		// POST: api/MedicalInventory
		[HttpPost]
		public async Task<ActionResult<MedicalInventoryDTO>> CreateInventoryItem(CreateMedicalInventoryRequest request)
		{
			var item = new MedicalInventory
			{
				ItemName = request.ItemName,
				Category = request.Category,
				Quantity = request.Quantity,
				Unit = request.Unit,
				Description = request.Description
			};

			_context.MedicalInventory.Add(item);
			await _context.SaveChangesAsync();

			return CreatedAtAction(
				nameof(GetInventoryItem),
				new { id = item.ItemID },
				new MedicalInventoryDTO
				{
					ItemID = item.ItemID,
					ItemName = item.ItemName,
					Category = item.Category,
					Quantity = item.Quantity,
					Unit = item.Unit,
					Description = item.Description
				});
		}

		// PUT: api/MedicalInventory/5
		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateInventoryItem(int id, CreateMedicalInventoryRequest request)
		{
			var item = await _context.MedicalInventory.FindAsync(id);
			if (item == null)
				return NotFound();

			item.ItemName = request.ItemName;
			item.Category = request.Category;
			item.Quantity = request.Quantity;
			item.Unit = request.Unit;
			item.Description = request.Description;

			await _context.SaveChangesAsync();
			return NoContent();
		}

		// DELETE: api/MedicalInventory/5
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteInventoryItem(int id)
		{
			var item = await _context.MedicalInventory.FindAsync(id);
			if (item == null)
				return NotFound();

			_context.MedicalInventory.Remove(item);
			await _context.SaveChangesAsync();

			return NoContent();
		}
	}
}