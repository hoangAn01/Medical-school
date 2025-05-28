using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolMedical.Core.Entities
{
    [Table("ManagerAdmin")]
    public class ManagerAdmin
    {
        [Key]
        public int ManagerID { get; set; }
        public string? FullName { get; set; }
        public char? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Address { get; set; }
        public int? UserID { get; set; }

        // Navigation properties
        public virtual Account? Account { get; set; }
    }

    [Table("Nurse")]
    public class Nurse
    {
        [Key]
        public int NurseID { get; set; }
        public string? FullName { get; set; }
        public char? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Phone { get; set; }
        public int? UserID { get; set; }

        // Navigation properties
        public virtual Account? Account { get; set; }
    }

    [Table("Parent")]
    public class Parent
    {
        [Key]
        public int ParentID { get; set; }
        public int? UserID { get; set; }
        public string? FullName { get; set; }
        public char? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }

        // Navigation properties
        public virtual Account? Account { get; set; }
    }

    [Table("Student")]
    public class Student
    {
        [Key]
        public int StudentID { get; set; }
        public string? FullName { get; set; }
        public char? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int? ParentID { get; set; }
        public int? UserID { get; set; }
        public int? ClassID { get; set; }

        // Navigation properties
        public virtual Account? Account { get; set; }
        public virtual Parent? Parent { get; set; }
        public virtual Class? Class { get; set; }
    }

    [Table("Class")]
    public class Class
    {
        [Key]
        public int ClassID { get; set; }
        public string ClassName { get; set; } = string.Empty;
        public string? SchoolYear { get; set; }
        public string? TeacherName { get; set; }
        public string? Description { get; set; }
        public int? TeacherID { get; set; }

        // Navigation properties
        public virtual ICollection<Student> Students { get; set; } = new List<Student>();
    }

    // Add other entities as needed for the complete system
    public class HealthProfile { }
    public class HealthReport { }
    public class MedicalEvent { }
    public class SchoolCheckup { }
    public class VaccinationEvent { }
    public class VaccineRecord { }
    public class MedicalInventory { }
    public class MedicalUsage { }
    public class MedicineRequest { }
    public class Allergen { }
    public class StudentAllergy { }
    public class Notification { }
}