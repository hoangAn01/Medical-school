using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolMedical.Core.DTOs.MedicalEvent;
using SchoolMedical.Core.Entities;
using SchoolMedical.Infrastructure.Data;
namespace SchoolMedical.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class MedicalEventsController : ControllerBase
	{
		private readonly ApplicationDbContext _context;

		public MedicalEventsController(ApplicationDbContext context)
		{
			_context = context;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<MedicalEventDto>>> GetMedicalEvents()
		{
			var events = await _context.MedicalEvents
				.Include(me => me.Student)
				.Select(me => new MedicalEventDto
				{
					EventID = me.EventID,
					StudentID = me.StudentID,
					StudentName = me.Student.FullName,
					EventType = me.EventType,
					EventTime = me.EventTime,
					Description = me.Description,
					Status = me.Status
				})
				.ToListAsync();

			return Ok(events);
		}

		[HttpPost]
		public async Task<IActionResult> CreateMedicalEvent([FromBody] MedicalEventCreateDto dto)
		{
			// 1. Tạo MedicalEvent
			var medicalEvent = new MedicalEvent
			{
				StudentID = dto.StudentID,
				EventType = dto.EventType,
				Description = dto.Description,
				EventTime = dto.EventTime,
				NurseID = dto.NurseID,
				Status = "Đang xử lý"
			};
			_context.MedicalEvents.Add(medicalEvent);
			await _context.SaveChangesAsync();

			// 2. Lấy thông tin học sinh và phụ huynh
			var student = await _context.Students.FindAsync(dto.StudentID);
			if (student != null && student.ParentID.HasValue)
			{
				// 3. Tạo Notification (nội dung thông báo)
				var notification = new Notification
				{
					Title = "Thông báo sự cố y tế",
					Content = $"Con bạn {student.FullName} gặp sự cố: {dto.EventType} - {dto.Description} vào lúc {dto.EventTime:dd/MM/yyyy HH:mm}",
					SentDate = DateTime.Now,
					Status = "Published",
					NotificationType = "MEDICAL_EVENT",
					MedicalEventID = medicalEvent.EventID
				};
				_context.Notifications.Add(notification);
				await _context.SaveChangesAsync();

				// 4. Tạo ParentNotification (gửi cho phụ huynh)
				var parentNotification = new ParentNotification
				{
					ParentID = student.ParentID.Value,
					NotificationID = notification.NotificationID,
					IndividualSentDate = DateTime.Now,
					IndividualStatus = "Sent"
				};
				_context.ParentNotifications.Add(parentNotification);
				await _context.SaveChangesAsync();
			}

			return CreatedAtAction(nameof(GetMedicalEventById), new { id = medicalEvent.EventID }, medicalEvent);
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<MedicalEventDto>> GetMedicalEventById(int id)
		{
			var medicalEvent = await _context.MedicalEvents
				.Include(me => me.Student)
				.Where(me => me.EventID == id)
				.Select(me => new MedicalEventDto
				{
					EventID = me.EventID,
					StudentID = me.StudentID,
					StudentName = me.Student.FullName,
					EventType = me.EventType,
					EventTime = me.EventTime,
					Description = me.Description,
					Status = me.Status
				})
				.FirstOrDefaultAsync();

			if (medicalEvent == null)
				return NotFound();

			return Ok(medicalEvent);
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteMedicalEvent(int id)
		{
			var medicalEvent = await _context.MedicalEvents.FindAsync(id);
			if (medicalEvent == null)
				return NotFound();

			_context.MedicalEvents.Remove(medicalEvent);
			await _context.SaveChangesAsync();
			return NoContent();
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateMedicalEvent(int id, [FromBody] MedicalEventUpdateDto dto)
		{
			var medicalEvent = await _context.MedicalEvents.FindAsync(id);
			if (medicalEvent == null)
				return NotFound();

			medicalEvent.EventType = dto.EventType;
			medicalEvent.Description = dto.Description;
			medicalEvent.EventTime = dto.EventTime;
			medicalEvent.NurseID = dto.NurseID;

			await _context.SaveChangesAsync();
			return NoContent();
		}

		[HttpGet("search")]
		public async Task<ActionResult<IEnumerable<MedicalEventDto>>> SearchMedicalEvents(
			[FromQuery] string? keyword)
		{
			var query = _context.MedicalEvents.Include(me => me.Student).AsQueryable();

			if (!string.IsNullOrEmpty(keyword))
			{
				query = query.Where(me =>
					me.Student.FullName.Contains(keyword) ||
					me.StudentID.ToString().Contains(keyword));
			}

			var events = await query
				.Select(me => new MedicalEventDto
				{
					EventID = me.EventID,
					StudentID = me.StudentID,
					StudentName = me.Student.FullName,
					EventType = me.EventType,
					EventTime = me.EventTime,
					Description = me.Description,
					Status = me.Status
				})
				.ToListAsync();

			return Ok(events);
		}

		[HttpPost("{id}/use-items")]
		public async Task<IActionResult> UseMedicalItems(int id, [FromBody] UseItemsDto dto)
		{
			var medicalEvent = await _context.MedicalEvents.FindAsync(id);
			if (medicalEvent == null)
				return NotFound("Sự kiện y tế không tồn tại");

			using var transaction = await _context.Database.BeginTransactionAsync();
			try
			{
				foreach (var item in dto.Items)
				{
					// Kiểm tra và cập nhật số lượng trong kho
					var inventory = await _context.MedicalInventory
						.FirstOrDefaultAsync(i => i.ItemID == item.ItemId);

					if (inventory == null)
						return NotFound($"Không tìm thấy vật tư y tế có ID {item.ItemId}");

					if (inventory.Quantity < item.Quantity)
						return BadRequest($"Số lượng {inventory.ItemName} trong kho không đủ");

					// Ghi nhận việc sử dụng vật tư
					var eventInventory = new MedicalEventInventory
					{
						EventID = id,
						ItemID = item.ItemId,
						QuantityUsed = item.Quantity,
						UsedTime = DateTime.Now
					};
					_context.MedicalEventInventory.Add(eventInventory);

					// Cập nhật số lượng trong kho
					inventory.Quantity -= item.Quantity;
				}

				// Cập nhật trạng thái sự kiện
				medicalEvent.Status = "Đã xử lý";

				await _context.SaveChangesAsync();
				await transaction.CommitAsync();

				return Ok("Đã cập nhật sử dụng vật tư y tế thành công");
			}
			catch (Exception)
			{
				await transaction.RollbackAsync();
				return StatusCode(500, "Đã xảy ra lỗi khi cập nhật sử dụng vật tư y tế");
			}
		}

		[HttpGet("student/{studentId}")]
		public async Task<ActionResult<IEnumerable<MedicalEventDto>>> GetMedicalEventsByStudentId(int studentId)
		{
			var events = await _context.MedicalEvents
				.Include(me => me.Student)
				.Where(me => me.StudentID == studentId)
				.Select(me => new MedicalEventDto
				{
					EventID = me.EventID,
					StudentID = me.StudentID,
					StudentName = me.Student.FullName,
					EventType = me.EventType,
					EventTime = me.EventTime,
					Description = me.Description,
					Status = me.Status
				})
				.ToListAsync();

			return Ok(events);
		}

		[HttpPut("{id}/status")]
		public async Task<IActionResult> UpdateMedicalEventStatus(int id, [FromBody] string status)
		{
			var medicalEvent = await _context.MedicalEvents.FindAsync(id);
			if (medicalEvent == null)
				return NotFound();

			medicalEvent.Status = status;
			await _context.SaveChangesAsync();
			return NoContent();
		}
	}

	public class MedicalEventUpdateDto
	{
		public string? EventType { get; set; }
		public string? Description { get; set; }
		public DateTime EventTime { get; set; }
		public int? NurseID { get; set; }
	}

// DTO cho việc sử dụng vật tư y tế
public class UseItemsDto
{
    public List<EventItemDto> Items { get; set; }
}

public class EventItemDto
{
    public int ItemId { get; set; }
    public int Quantity { get; set; }
}

}
