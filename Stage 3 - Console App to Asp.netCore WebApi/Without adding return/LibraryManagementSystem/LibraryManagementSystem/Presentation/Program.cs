


using LibraryManagementSystem.Infrastructure.Data;
using LibraryManagementSystem.Infrastructure.Repositories;
using LibraryManagementSystem.UseCases;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// 🔹 Configure SQL Server connection
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=LibraryDB;Trusted_Connection=True;"));

// 🔹 Register services
builder.Services.AddScoped<IBookRepository,SqlBookRepository>();
builder.Services.AddScoped<LibraryService>();

// 🔹 Enable controllers
builder.Services.AddControllers();

// 🔹 Enable Swagger for API testing
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1",new OpenApiInfo { Title = "Library API", Version = "v1" });
});

var app = builder.Build();

// 🔹 Enable Swagger UI
//if (app.Environment.IsDevelopment()) // this will enable production
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseAuthorization();
app.MapControllers();
app.Run();