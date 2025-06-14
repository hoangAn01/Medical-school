using Microsoft.EntityFrameworkCore;
using SchoolMedical.Core.Entities;

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
                entity.Property(e => e.MedicineName).IsRequired().HasMaxLength(100);
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
        }
    }
}