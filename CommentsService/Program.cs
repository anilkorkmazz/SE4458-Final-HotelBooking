using CommentsService.Models;
using CommentsService.Repositories;
using CommentsService.Services;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc; // <-- Bunu ekle

var builder = WebApplication.CreateBuilder(args);

// 🔧 MongoDB Ayarlarını Yükle
builder.Services.Configure<MongoDBSettings>(
    builder.Configuration.GetSection("MongoDBSettings"));

// ⛓️ Dependency Injection
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<ICommentService, CommentService>();

// 🚫 Varsayılan model state hatasını devre dışı bırak
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

// 📦 Controller ve Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 🌐 CORS (İsteğe Bağlı: Frontend erişimi için)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

var app = builder.Build();

// 🚀 Middleware
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();

app.Run();
