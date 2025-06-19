using System;
using System.ComponentModel.DataAnnotations;

namespace SchoolMedical.Core.DTOs
{
	// DTO để trả về dữ liệu Blog
	public class BlogDto
	{
		public int BlogID { get; set; }
		public string Title { get; set; }
		public string Content { get; set; }
		public string? ImageUrl { get; set; }
		public int AuthorID { get; set; }
		public string AuthorUsername { get; set; }
		public string AuthorFullName { get; set; }
		public DateTime? CreatedDate { get; set; }
		public DateTime? UpdatedDate { get; set; }
		public bool IsPublished { get; set; }
	}

	// Model để tạo Blog mới (nhận từ request body)
	public class BlogCreateModel
	{
		[Required]
		[MaxLength(255)]
		public string Title { get; set; }

		[Required]
		public string Content { get; set; }

		[MaxLength(500)]
		public string? ImageUrl { get; set; }
	}

	// Model để cập nhật Blog (nhận từ request body)
	public class BlogUpdateModel
	{
		[Required]
		public int BlogID { get; set; }

		[Required]
		[MaxLength(255)]
		public string Title { get; set; }

		[Required]
		public string Content { get; set; }

		[MaxLength(500)]
		public string? ImageUrl { get; set; }
	}
} 