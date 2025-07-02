using AIAgentService.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using HotelService.Services;
using HotelService.Repositories;

using Microsoft.EntityFrameworkCore;
using HotelService.Data; // HotelDbContext burada tanımlı


var builder = WebApplication.CreateBuilder(args);

// ⬇️ Agent servisleri
builder.Services.AddHttpClient<IAgentService, AgentService>();
builder.Services.AddHttpClient<IAgentCoordinatorService, AgentCoordinatorService>();

/*
builder.Services.AddScoped<IHotelService, HotelService.Services.HotelService>();
builder.Services.AddScoped<IRoomService, HotelService.Services.RoomService>();

builder.Services.AddScoped<IHotelRepository, HotelService.Repositories.HotelRepository>();
builder.Services.AddScoped<IRoomRepository, HotelService.Repositories.RoomRepository>();
*/


// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// API Versioning
builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ReportApiVersions = true;
});

// Controller + Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Swagger UI
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// -------------------------
// ✅ Database Configuration
// -------------------------

/*
builder.Services.AddDbContext<HotelDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 36))
    ));
*/

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();
app.Run();
