using API.Repositories;
using API.Services;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Serilog;


var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug() // Set minimum log level to Debug
    .WriteTo.Console() // Log to the console
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day) // Log to a file
    .CreateLogger();

builder.Host.UseSerilog(); // Use Serilog for logging

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure the DbContext with PostgreSQL connection string
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped(typeof(GenericRepository<>), typeof(GenericRepositoryImpl<>));
builder.Services.AddScoped(typeof(TicketService), typeof(TicketServiceImpl));

// Add CORS services
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowOrigin", policy =>
    {
        policy.WithOrigins("http://localhost:3000")  // Replace with your frontend's URL
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});


// Register Controllers
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors("AllowOrigin");  // Enable CORS

app.UseAuthorization();

app.MapControllers();

app.UseSerilogRequestLogging(); // Enable Serilog request logging

app.Run();
