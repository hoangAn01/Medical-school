using Microsoft.EntityFrameworkCore;
using SchoolMedical.Core.Entities;
using SchoolMedical.Infrastructure.Data;
using System.Text;

namespace SchoolMedical.Infrastructure.Services
{
	public interface INotificationService
	{
		Task SendMedicineRequestNotificationAsync(int medicineRequestId, string status, string nurseNote = "");
	}

	public class NotificationService : INotificationService
	{
		private readonly ApplicationDbContext _context;
		
		public NotificationService(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task SendMedicineRequestNotificationAsync(int medicineRequestId, string status, string nurseNote = "")
		{
			// Get the medicine request with related data
			var medicineRequest = await _context.MedicineRequests
				.Include(m => m.Student)
				.Include(m => m.Parent)
				.Include(m => m.MedicineRequestDetails)
				.ThenInclude(d => d.RequestItem)
				.FirstOrDefaultAsync(m => m.RequestID == medicineRequestId);

			if (medicineRequest == null || medicineRequest.ParentID == null)
				return;

			// Create notification title and content
			var (title, content) = CreateNotificationContent(medicineRequest, status, nurseNote);

			// Create the notification
			var notification = new Notification
			{
				Title = title,
				Content = content,
				SentDate = DateTime.UtcNow,
				Status = "Sent"
			};

			_context.Notifications.Add(notification);
			await _context.SaveChangesAsync();

			// Create parent notification relationship
			var parentNotification = new ParentNotification
			{
				NotificationID = notification.NotificationID,
				ParentID = medicineRequest.ParentID.Value,
				IndividualSentDate = DateTime.UtcNow,
				IndividualStatus = "Sent"
			};

			_context.ParentNotifications.Add(parentNotification);
			await _context.SaveChangesAsync();
		}

		private (string title, string content) CreateNotificationContent(MedicineRequest request, string status, string nurseNote)
		{
			var studentName = request.Student?.FullName?? "Không xác định";
			var statusText = status == "Approved" ? "được phê duyệt" : "bị từ chối";
			
			var title = $"Yêu cầu thuốc {status} - {studentName}";
			
			var contentBuilder = new StringBuilder();
			contentBuilder.AppendLine($"Kính gửi phụ huynh,");
			contentBuilder.AppendLine();
			contentBuilder.AppendLine($"Yêu cầu thuốc của {studentName} đã được {statusText}.");
			contentBuilder.AppendLine();
			contentBuilder.AppendLine($"Chi tiết yêu cầu:");
			contentBuilder.AppendLine($"- Ngày yêu cầu: {request.Date:dd/MM/yyyy}");
			contentBuilder.AppendLine($"- Trạng thái: {status}");
			
			if (request.ApprovalDate.HasValue)
				contentBuilder.AppendLine($"- Ngày phê duyệt: {request.ApprovalDate.Value:dd/MM/yyyy}");

			// Add medicine details
			if (request.MedicineRequestDetails.Any())
			{
				contentBuilder.AppendLine();
				contentBuilder.AppendLine("Yêu cầu thuốc:");
				foreach (var detail in request.MedicineRequestDetails)
				{
					contentBuilder.AppendLine($"- {detail.RequestItem?.RequestItemName}: {detail.Quantity} đơn vị");
					if (!string.IsNullOrEmpty(detail.DosageInstructions))
						contentBuilder.AppendLine($"  Liều dùng: {detail.DosageInstructions}");

					if (!string.IsNullOrEmpty(detail.Time))
						contentBuilder.AppendLine($"  Thời gian uống thuốc: {detail.Time}");
				}
			}

			// Add parent's original note if exists
			if (!string.IsNullOrWhiteSpace(request.Note))
			{
				contentBuilder.AppendLine();
				contentBuilder.AppendLine($"Ghi chú của phụ huynh: {request.Note}");
			}

			// Add nurse's note if exists
			if (!string.IsNullOrWhiteSpace(nurseNote))
			{
				contentBuilder.AppendLine();
				contentBuilder.AppendLine($"Ghi chú của nhân viên y tế: {nurseNote}");
			}

			contentBuilder.AppendLine();
			contentBuilder.AppendLine("Cảm ơn sự hợp tác của quý phụ huynh.");
			contentBuilder.AppendLine("Đội ngũ y tế trường học");

			return (title, contentBuilder.ToString());
		}
	}
} 