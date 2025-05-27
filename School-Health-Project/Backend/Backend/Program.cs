using DemoBackend.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection; // Ensure this is included
using Microsoft.OpenApi.Models; // Add this for Swagger

internal class Program
{
	private static void Main(string[] args)
	{
		var builder = WebApplication.CreateBuilder(args);

		// Add services to the container
		builder.Services.AddControllers();

		// 'DbContextOptionsBuilder' does not contain a definition for 'UseSqlServer' and no accessible extension method 'UseSqlServer' accepting a first argument of type 'DbContextOptionsBuilder' could be found (are you missing a using directive or an assembly reference?)
		builder.Services.AddDbContext<DataContext>(options =>
			options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

		builder.Services.AddEndpointsApiExplorer();
		// builder.Services.AddSwaggerGen(); -> Error ???

		builder.Services.AddCors(options =>{
			options.AddPolicy("AllowFrontend", policy =>{
				policy.WithOrigins("http://localhost:3000")
				  .AllowAnyHeader()
				  .AllowAnyMethod();
			});
		});

		var app = builder.Build();

		// DB Connection Test
		using (var scope = app.Services.CreateScope()){
			var context = scope.ServiceProvider.GetRequiredService<DataContext>();
			if (context.CanConnectToDatabase())
				Console.WriteLine("✅ Successfully connected to the database.");
			else
				Console.WriteLine("❌ Failed to connect to the database.");
		}

		// Middleware
		if (app.Environment.IsDevelopment()) app.UseSwagger();
		app.UseSwaggerUI(); 

		app.UseHttpsRedirection();

		app.UseCors("AllowFrontend");

		app.UseAuthorization();

		app.MapControllers();

		app.Run();
	}
}