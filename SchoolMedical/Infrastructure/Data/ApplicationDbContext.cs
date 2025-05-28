using Microsoft.EntityFrameworkCore;
using SchoolMedical.Core.Entities;

namespace SchoolMedical.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // DbSets
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Parent> Parents { get; set; }
        public DbSet<Nurse> Nurses { get; set; }
        public DbSet<ManagerAdmin> ManagerAdmins { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<HealthProfile> HealthProfiles { get; set; }
        public DbSet<HealthReport> HealthReports { get; set; }
        public DbSet<MedicalEvent> MedicalEvents { get; set; }
        public DbSet<SchoolCheckup> SchoolCheckups { get; set; }
        public DbSet<VaccinationEvent> VaccinationEvents { get; set; }
        public DbSet<VaccineRecord> VaccineRecords { get; set; }
        public DbSet<MedicalInventory> MedicalInventories { get; set; }
        public DbSet<MedicalUsage> MedicalUsages { get; set; }
        public DbSet<MedicineRequest> MedicineRequests { get; set; }
        public DbSet<Allergen> Allergens { get; set; }
        public DbSet<StudentAllergy> StudentAllergies { get; set; }
        public DbSet<Notification> Notifications { get; set; }

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

            // Configure relationships
            modelBuilder.Entity<ManagerAdmin>()
                .HasOne(m => m.Account)
                .WithOne(a => a.ManagerAdmin)
                .HasForeignKey<ManagerAdmin>(m => m.UserID);

            modelBuilder.Entity<Nurse>()
                .HasOne(n => n.Account)
                .WithOne(a => a.Nurse)
                .HasForeignKey<Nurse>(n => n.UserID);

            modelBuilder.Entity<Parent>()
                .HasOne(p => p.Account)
                .WithOne(a => a.Parent)
                .HasForeignKey<Parent>(p => p.UserID);

            // Add other entity configurations as needed
        }
    }
}