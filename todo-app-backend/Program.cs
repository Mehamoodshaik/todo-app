using Microsoft.EntityFrameworkCore;
using TodoApp.Data; 

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add MySQL DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
    new MySqlServerVersion(new Version(8, 0, 25))));

// Add CORS Policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowVueApp",
        policy => policy.WithOrigins(
            "http://localhost:5173",
            "http://localhost:5174" 
        ).AllowAnyHeader().AllowAnyMethod());
});
var app = builder.Build();

// Configure the HTTP request pipeline
app.UseCors("AllowVueApp");
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
