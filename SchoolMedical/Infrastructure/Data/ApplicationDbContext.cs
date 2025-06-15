using Microsoft.EntityFrameworkCore;
using SchoolMedical.Core.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolMedical.Infrastructure.Data
{
	public class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

		// DbSets
		public DbSet<Account> Accounts { get; set; }
		public DbSet<Student> Students { get; set; }
		public DbSet<Parent> Parents { get; set; }
		public DbSet<Nurse> Nurses { get; set; }
		public DbSet<ManagerAdmin> ManagerAdmins { get; set; }
		public DbSet<Class> Classes { get; set; }
		public DbSet<HealthProfile> HealthProfiles { get; set; }
		public DbSet<MedicineRequest> MedicineRequests { get; set; }
		public DbSet<MedicalInventory> MedicalInventory { get; set; }
		public DbSet<MedicalEvent> MedicalEvents { get; set; }
		public DbSet<Notification> Notifications { get; set; }
		public DbSet<ParentNotification> ParentNotifications { get; set; }
		public DbSet<ParentalConsent> ParentalConsents { get; set; }
		public DbSet<VaccinationEvent> VaccinationEvents { get; set; }
		public DbSet<MedicineRequestDetail> MedicineRequestDetails { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			// Configure Account entity
			modelBuilder.Entity<Account>(entity =>
			{
				entity.HasKey(e => e.UserID);
				entity.Property(e => e.Username).IsRequired().HasMaxLength(100);
				entity.Property(e => e.PasswordHash).IsRequired().HasMaxLength(255);
				entity.Property(e => e.Role).IsRequired().HasMaxLength(50);

				// Create unique index on Username
				entity.HasIndex(e => e.Username).IsUnique();
			});

			// Configure ManagerAdmin entity
			modelBuilder.Entity<ManagerAdmin>(entity =>
			{
				entity.HasKey(e => e.ManagerID);
				entity.Property(e => e.FullName).HasMaxLength(100);
				entity.Property(e => e.Gender).HasMaxLength(1);
				entity.Property(e => e.Address).HasMaxLength(255);

				// Configure relationship with Account
				entity.HasOne(m => m.Account)
					  .WithOne()
					  .HasForeignKey<ManagerAdmin>(m => m.UserID)
					  .OnDelete(DeleteBehavior.Restrict);
			});

			// Configure Nurse entity
			modelBuilder.Entity<Nurse>(entity =>
			{
				entity.HasKey(e => e.NurseID);
				entity.Property(e => e.FullName).HasMaxLength(100);
				entity.Property(e => e.Gender).HasMaxLength(1);
				entity.Property(e => e.Phone).HasMaxLength(20);

				// Configure relationship with Account
				entity.HasOne(n => n.Account)
					  .WithOne()
					  .HasForeignKey<Nurse>(n => n.UserID)
					  .OnDelete(DeleteBehavior.Restrict);
			});

			// Configure Parent entity
			modelBuilder.Entity<Parent>(entity =>
			{
				entity.HasKey(e => e.ParentID);
				entity.Property(e => e.FullName).HasMaxLength(100);
				entity.Property(e => e.Gender).HasMaxLength(1);
				entity.Property(e => e.Address).HasMaxLength(255);
				entity.Property(e => e.Phone).HasMaxLength(20);

				// Configure relationship with Account
				entity.HasOne(p => p.Account)
					  .WithOne()
					  .HasForeignKey<Parent>(p => p.UserID)
					  .OnDelete(DeleteBehavior.Restrict);
			});

			// Configure Student entity
			modelBuilder.Entity<Student>(entity =>
			{
				entity.HasKey(e => e.StudentID);
				entity.Property(e => e.FullName).HasMaxLength(100);
				entity.Property(e => e.Gender).HasMaxLength(1);

				// Configure relationship with Account (optional)
				entity.HasOne(s => s.Account)
					  .WithOne()
					  .HasForeignKey<Student>(s => s.UserID)
					  .OnDelete(DeleteBehavior.Restrict);

				// Configure relationship with Parent
				entity.HasOne(s => s.Parent)
					  .WithMany()
					  .HasForeignKey(s => s.ParentID)
					  .OnDelete(DeleteBehavior.Restrict);

				// Configure relationship with Class
				entity.HasOne(s => s.Class)
					  .WithMany(c => c.Students)
					  .HasForeignKey(s => s.ClassID)
					  .OnDelete(DeleteBehavior.Restrict);
			});

			// Configure Class entity
			modelBuilder.Entity<Class>(entity =>
			{
				entity.HasKey(e => e.ClassID);
				entity.Property(e => e.ClassName).IsRequired().HasMaxLength(50);
				entity.Property(e => e.SchoolYear).HasMaxLength(20);
				entity.Property(e => e.TeacherName).HasMaxLength(100);
				entity.Property(e => e.Description).HasMaxLength(255);
			});

			// Configure HealthProfile entity
			modelBuilder.Entity<HealthProfile>(entity =>
			{
				entity.HasKey(e => e.ProfileID);
				entity.Property(e => e.ChronicDisease).HasMaxLength(255);
				entity.Property(e => e.VisionTest).HasMaxLength(255);
				entity.Property(e => e.Allergy).HasMaxLength(255);
				entity.Property(e => e.Weight).HasColumnType("decimal(5,2)");
				entity.Property(e => e.Height).HasColumnType("decimal(5,2)");
				entity.Property(e => e.LastCheckupDate).HasColumnType("date");

				entity.HasOne(e => e.Student)
					.WithMany()
					.HasForeignKey(e => e.StudentID)
					.OnDelete(DeleteBehavior.SetNull);
			});

			// Configure MedicineRequest entity
			modelBuilder.Entity<MedicineRequest>(entity =>
			{
				entity.HasKey(e => e.RequestID);
				entity.Property(e => e.Date).HasColumnType("date").IsRequired();
				entity.Property(e => e.RequestStatus).HasMaxLength(50);
				entity.Property(e => e.Note).HasMaxLength(255);
				entity.Property(e => e.ApprovalDate).HasColumnType("date");

				// Configure relationship with Student (Required)
				entity.HasOne(m => m.Student)
					.WithMany()
					.HasForeignKey(m => m.StudentID)
					.OnDelete(DeleteBehavior.Restrict);

				// Configure relationship with Parent (Optional)
				entity.HasOne(m => m.Parent)
					.WithMany()
					.HasForeignKey(m => m.ParentID)
					.OnDelete(DeleteBehavior.Restrict);

				// Configure relationship with Nurse (Optional, for ApprovedBy)
				entity.HasOne<Nurse>()
					.WithMany()
					.HasForeignKey(m => m.ApprovedBy)
					.OnDelete(DeleteBehavior.Restrict);
			});

			// Configure MedicineRequestDetail entity
			modelBuilder.Entity<MedicineRequestDetail>(entity =>
			{
				entity.HasKey(e => e.RequestDetailID);
				entity.Property(e => e.DosageInstructions).HasMaxLength(255);

				entity.HasOne(d => d.MedicineRequest)
					.WithMany(m => m.MedicineRequestDetails)
					.HasForeignKey(d => d.RequestID)
					.OnDelete(DeleteBehavior.Restrict);

				entity.HasOne(d => d.MedicalInventory)
					.WithMany()
					.HasForeignKey(d => d.ItemID)
					.OnDelete(DeleteBehavior.Restrict);
			});

			// Configure Notification entity (updated for many-to-many)
			modelBuilder.Entity<Notification>(entity =>
			{
				entity.HasKey(e => e.NotificationID);
				entity.Property(e => e.Title).IsRequired().HasMaxLength(100);
				entity.Property(e => e.Content).IsRequired();
				entity.Property(e => e.SentDate).IsRequired();
				entity.Property(e => e.Status).HasMaxLength(50);
			});

			// Configure ParentNotification entity (join table)
			modelBuilder.Entity<ParentNotification>(entity =>
			{
				entity.HasKey(e => new { e.NotificationID, e.ParentID });

				entity.Property(e => e.IndividualSentDate).IsRequired();
				entity.Property(e => e.IndividualStatus).HasMaxLength(50);

				entity.HasOne(pn => pn.Notification)
					.WithMany(n => n.ParentNotifications)
					.HasForeignKey(pn => pn.NotificationID)
					.OnDelete(DeleteBehavior.Cascade); // Cascade delete from Notification to ParentNotification

				entity.HasOne(pn => pn.Parent)
					.WithMany()
					.HasForeignKey(pn => pn.ParentID)
					.OnDelete(DeleteBehavior.Cascade); // Cascade delete from Parent to ParentNotification
			});

			// Configure ParentalConsent entity
			modelBuilder.Entity<ParentalConsent>(entity =>
			{
				entity.HasKey(e => e.ConsentID);
				entity.HasIndex(e => new { e.StudentID, e.VaccinationEventID }).IsUnique(); // Unique constraint
				entity.Property(e => e.ConsentStatus).IsRequired().HasMaxLength(50);

				entity.HasOne(pc => pc.Student)
					.WithMany()
					.HasForeignKey(pc => pc.StudentID)
					.OnDelete(DeleteBehavior.Restrict);

				entity.HasOne(pc => pc.VaccinationEvent)
					.WithMany()
					.HasForeignKey(pc => pc.VaccinationEventID)
					.OnDelete(DeleteBehavior.Restrict);

				entity.HasOne(pc => pc.Parent)
					.WithMany()
					.HasForeignKey(pc => pc.ParentID)
					.OnDelete(DeleteBehavior.Restrict);
			});

		}
	}
}