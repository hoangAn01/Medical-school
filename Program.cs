// ...existing using statements...
var builder = WebApplication.CreateBuilder(args);

// ...existing code...

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.WithOrigins("http://localhost:3000") // Change to your frontend URL/port
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

// ...existing code...

var app = builder.Build();

// ...existing code...

// Use CORS
app.UseCors("AllowFrontend");

// ...existing code...
app.UseAuthorization();
// ...existing code...
app.MapControllers();
// ...existing code...
app.Run();