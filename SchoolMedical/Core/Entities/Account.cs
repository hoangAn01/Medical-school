﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolMedical.Core.Entities
{
	[Table("Account")]
	public class Account
	{
		[Key]
		// [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int UserID { get; set; }

		[Required]
		[StringLength(100)]
		public string Username { get; set; } = string.Empty;

		[Required]
		[StringLength(255)]
		public string PasswordHash { get; set; } = string.Empty;

		[Required]
		[StringLength(50)]
		public string Role { get; set; } = string.Empty;

		//[Required]
		//[StringLength(100)]
		//public string FullName { get; set; } = string.Empty;

		public bool Active { get; set; } = true;

	}
}
