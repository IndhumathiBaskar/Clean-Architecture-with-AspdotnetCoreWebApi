using LibraryManagementSystem.Entities;
using LibraryManagementSystem.UseCases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Infrastructure
{
    public class InMemoryBookRepository : IBookRepository
    {

        private readonly List<Book> _books = new()
        {
            new Book("C# Basics", "John"),
            new Book("ASP.NET Core", "Jane Smith")
        };
        public Book GetBookByTitle(string title)
        {
            return _books.FirstOrDefault(b => b.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
        }

        public void UpdateBook(Book book)
        {
            // Since it's an in-memory list, changes are automatically saved
        }
    }
}
