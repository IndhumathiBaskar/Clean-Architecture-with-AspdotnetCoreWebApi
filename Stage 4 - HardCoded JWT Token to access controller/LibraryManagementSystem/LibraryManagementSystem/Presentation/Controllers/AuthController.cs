using LibraryManagementSystem.UseCases;
using Microsoft.AspNetCore.Mvc;


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
            if(request.Username == "admin" && request.Password == "password123")
            {
                var token = _authService.GenerateToken(request.Username);
                return Ok(new { token });
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