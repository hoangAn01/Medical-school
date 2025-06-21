using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolMedical.Core.Entities
{
	[Table("RequestItemList")]
	public class RequestItemList
	{
		[Key]
		public int RequestItemID { get; set; }
		
		[Required]
		[StringLength(255)]
		public string RequestItemName { get; set; } = string.Empty;
		
		[StringLength(500)]
		public string? Description { get; set; }
	}
} 