using LibraryManagementSystem.Infrastructure;
using LibraryManagementSystem.Infrastructure.Data;
using LibraryManagementSystem.Infrastructure.Repositories;
using LibraryManagementSystem.UseCases;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace LibraryManagementSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            /* Step-by-Step Explanation

                a. Create a ServiceCollection Instance

                var serviceProvider = new ServiceCollection()
                1. ServiceCollection is a container where we register services (objects we need in our app).
                2. new ServiceCollection() creates a new DI container.

                b. Register the Database Context (AppDbContext)

                .AddDbContext<AppDbContext>(options =>
                    options.UseSqlServer("Server=YOUR_SERVER;Database=LibraryDB;Trusted_Connection=True;"))
	
                1. AddDbContext<AppDbContext> registers the AppDbContext class in DI.
                2. options.UseSqlServer(...) tells Entity Framework Core to use SQL Server as the database.
                3. "Server=YOUR_SERVER;Database=LibraryDB;Trusted_Connection=True;" is the connection string for SQL Server.

                c. Register the Repository (IBookRepository → SqlBookRepository)

                .AddScoped<IBookRepository, SqlBookRepository>()
                1. This registers IBookRepository (an interface) and links it to SqlBookRepository (a concrete class).
                2. .AddScoped<IBookRepository, SqlBookRepository>() means that for each HTTP request (in a web app), a new instance of SqlBookRepository will be created.

                d. Register the Library Service

                .AddScoped<LibraryService>()
                1. This registers the LibraryService class in DI.
                2. LibraryService might use IBookRepository inside it to manage books.

                e. Build the Service Provider

                .BuildServiceProvider();

                1. This finalizes the DI container setup.
                2 .serviceProvider now has all the services registered and can be used to get instances. */

            var serviceProvider = new ServiceCollection()
                .AddDbContext<AppDbContext>(options =>
                    options.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=LibraryDB;Trusted_Connection=True;"))
                .AddScoped<IBookRepository, SqlBookRepository>()
                .AddScoped<LibraryService>()
                .BuildServiceProvider();

            var libraryService = serviceProvider.GetService<LibraryService>();

            if (libraryService == null)
            {
                Console.WriteLine("Error: Unable to resolve LibraryService. Ensure dependency injection is configured correctly.");
                return;
            }

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
