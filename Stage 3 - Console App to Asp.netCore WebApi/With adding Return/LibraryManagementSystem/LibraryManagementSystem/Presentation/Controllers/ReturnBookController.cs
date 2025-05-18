using LibraryManagementSystem.UseCases;
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
    public class ReturnBookController : ControllerBase 
    {
        private readonly LibraryService _libraryService;

        public ReturnBookController(LibraryService libraryService) 
        {
            _libraryService = libraryService;
        }

        [HttpPost("return/{title}")] 
        public IActionResult ReturnBook(string title) 
        {
            try
            {
                _libraryService.ReturnBook(title);
                return Ok(new { message = $"you have returned '{title}' book" }); 
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
