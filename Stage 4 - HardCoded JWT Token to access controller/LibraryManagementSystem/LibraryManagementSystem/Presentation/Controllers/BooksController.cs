using LibraryManagementSystem.UseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Presentation.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    [Authorize] // 🔒 Requires authentication
    public class BooksController : ControllerBase // BooksController is an API controller for books.
    {
        private readonly LibraryService _libraryService;

        public BooksController(LibraryService libraryService) 
        {
            _libraryService = libraryService;
        }

        [HttpPost("borrow/{title}")] //POST /api/books/borrow/{title} → Calls BorrowBook(title).
        public IActionResult BorrowBook(string title) 
        {
            try
            {
                _libraryService.BorrowBook(title);
                return Ok(new { message = $"you have borrowed '{title}'" }); // Returns success or error messages in JSON format
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message }); // Returns success or error messages in JSON format
            }
        }
    }
}
