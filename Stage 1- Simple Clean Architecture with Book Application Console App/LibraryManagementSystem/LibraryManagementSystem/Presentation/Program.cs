using LibraryManagementSystem.Infrastructure;
using LibraryManagementSystem.UseCases;
using System;

namespace LibraryManagementSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            IBookRepository bookRepository = new InMemoryBookRepository();
            LibraryService libraryService = new LibraryService(bookRepository);

            Console.WriteLine("Enter book title to borrow:");
            string title = Console.ReadLine();

            try
            {
                libraryService.BorrowBook(title);
                Console.WriteLine($"You have Borrowed '{title}'");
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            Console.ReadKey();
        }
    }
}
