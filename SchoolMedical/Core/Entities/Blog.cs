using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolMedical.Core.Entities
{
	[Table("Blog")]
	public class Blog
	{
		[Key]
		public int BlogID { get; set; }

		[Required]
		[MaxLength(255)]
		public string Title { get; set; }

		[Required]
		[Column(TypeName = "nvarchar(max)")]
		public string Content { get; set; }

		[MaxLength(500)]
		public string? ImageUrl { get; set; }

		[Required]
		public int AuthorID { get; set; } // Khóa ngoại tới Account.UserID

		public DateTime? CreatedDate { get; set; }

		public DateTime? UpdatedDate { get; set; }

		public bool IsPublished { get; set; }

		// Navigation property
		[ForeignKey("AuthorID")]
		public Account Account { get; set; }
	}
} 