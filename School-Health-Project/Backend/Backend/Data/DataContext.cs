using Microsoft.EntityFrameworkCore;
using Backend.Entity;

namespace DemoBackend.Data
{
	public class DataContext : DbContext
	{
		public DataContext(DbContextOptions<DataContext> options) : base(options) { }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<Account>().ToTable("Account");

			modelBuilder.Entity<HealthProfile>().ToTable("HealthProfile"); // ✅ Mapping đúng bảng
		}

		public DbSet<Account> Users { get; set; }
		public DbSet<HealthProfile> HealthProfiles { get; set; } // ✅ Thêm dòng này

		public bool CanConnectToDatabase(){
			try{
				return this.Database.CanConnect();
			}
			catch (Exception ex)
			{
				Console.WriteLine($"[Database Connection Error] {ex.Message}");
				return false;
			}
		}
	}
}
