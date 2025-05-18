using LibraryManagementSystem.Infrastructure.Data;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;
using LibraryManagementSystem.Entities;

namespace LibraryManagementSystem.UseCases
{
    public class AuthService
    {
        private readonly ApplicationDbContext _context;

        public AuthService(ApplicationDbContext context)
        {
            _context = context;
        }

        //Register() → Hashes password and stores the user in the database.
        public async Task<string> Register(string username, string password, string role)
        {
            if (await _context.Users.AnyAsync(u => u.UserName == username))
                return "Username already exists";

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(password); // hash Password

            var user = new User { UserName = username, PasswordHash = passwordHash , Role = role};
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return "User registered successfully";
        }

        //Login() → Verifies password and returns a JWT token if valid.
        public async Task<string> Login(string username,string password, JwtService jwtService)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
                return "Invalid username or password";

            return jwtService.GenerateToken(username, user.Role);
        }
    }
}
