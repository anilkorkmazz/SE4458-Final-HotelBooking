using Microsoft.AspNetCore.Hosting;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Ocelot konfigürasyonunu yükle
builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);

// ✅ CORS ayarlarını ekle
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Ocelot servislerini ekle
builder.Services.AddOcelot(builder.Configuration);

var app = builder.Build();

// ✅ CORS middleware'ini uygula
app.UseCors("AllowAll");

// Ocelot middleware'i başlat
await app.UseOcelot();

app.Run();
