using LibraryManagementSystem.UseCases;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;


namespace LibraryManagementSystem.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService) 
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {

            //HardCoded Users(Replace with database validation)

            var users = new Dictionary<string, string>
            {
                { "admin","Admin" },
                { "user" , "User" }
            };
            if(users.ContainsKey(request.Username) && request.Password == "password123")
            {
                var role = users[request.Username];
                var token = _authService.GenerateToken(request.Username, role);
                return Ok(new { token, role });
            }

            return Unauthorized(new { message = "Invalid credentials" });

        }

    }
}


public class LoginRequest
{
    public string Username { get; set; }
    public string Password { get; set; }
}