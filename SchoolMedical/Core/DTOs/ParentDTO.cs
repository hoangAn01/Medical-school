using System;

namespace SchoolMedical.Core.DTOs
{
    public class ParentDTO
    {
        public int ParentID { get; set; }
        public int? UserID { get; set; }
        public string? FullName { get; set; }
        public char? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
    }
}