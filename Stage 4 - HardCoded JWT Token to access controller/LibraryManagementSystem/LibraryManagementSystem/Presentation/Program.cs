


using LibraryManagementSystem.Infrastructure.Data;
using LibraryManagementSystem.Infrastructure.Repositories;
using LibraryManagementSystem.UseCases;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


/*
Configures JWT authentication using a secret key to validate and sign tokens.
- Defines a symmetric security key for signing JWT tokens (should be strong and kept secret).
- Adds authentication services using JWT Bearer as the default authentication and challenge scheme.
- Configures JWT Bearer authentication options:
  - Disables HTTPS metadata validation (only for development).
  - Enables token storage.
  - Sets token validation parameters:
    - Ensures the token's signature is validated using the secret key.
    - Disables issuer and audience validation (useful for simpler setups, but should be configured for production security).
*/

var key = Encoding.UTF8.GetBytes("aB9dF2!gH7jK1pQ3rS5tU8vX0yZ#cDeFgHiJkLmNoPqRsTuVwXyZ1234567890");  // 🔑 Replace with a strong key

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false,
    };
});

// 🔹 Configure SQL Server connection
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=LibraryDB;Trusted_Connection=True;"));

// 🔹 Register services
builder.Services.AddScoped<IBookRepository,SqlBookRepository>();
builder.Services.AddScoped<LibraryService>();

// 🔹 Register AuthService with the secret key
builder.Services.AddScoped<AuthService>(provider => new AuthService("aB9dF2!gH7jK1pQ3rS5tU8vX0yZ#cDeFgHiJkLmNoPqRsTuVwXyZ1234567890")); // Pass the secret key here


// 🔹 Enable controllers
builder.Services.AddControllers();

// 🔹 Enable Swagger for API testing
builder.Services.AddEndpointsApiExplorer();

/// <summary>
/// Configures Swagger to include JWT Bearer authentication support.
/// </summary>
/// <param name="c">The Swagger generator options.</param>
/// <remarks>
/// This code block performs the following:
///   1. Adds a security definition named "Bearer" to Swagger, describing how JWT Bearer authentication is used.
///   2. Specifies that the JWT token should be provided in the "Authorization" header.
///   3. Sets the security scheme type to HTTP with the "bearer" scheme and "JWT" format.
///   4. Adds a security requirement to apply the "Bearer" security definition to all endpoints, enabling the "Authorize" button in Swagger UI.
/// </remarks>

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1",new OpenApiInfo { Title = "Library API", Version = "v1" });

    // 🔑 Add Security Definition
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

    // 🔑 Add Security Requirement
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });

});

// 🔹 Enable Authorization
builder.Services.AddAuthorization();

var app = builder.Build();

// 🔹 Enable Swagger UI
//if (app.Environment.IsDevelopment()) // this will enable production
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

//Enables Authentication & Authorization middleware.
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();