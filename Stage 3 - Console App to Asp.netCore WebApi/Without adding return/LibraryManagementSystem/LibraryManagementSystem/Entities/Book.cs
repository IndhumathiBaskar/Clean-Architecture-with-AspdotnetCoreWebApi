using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Entities
{
    public class Book
    {
        public string Title { get; private set; }

        public string Author { get; private set; }

        public bool IsAvailable { get; private set; } = true;

        public Book(string title, string author)
        {
            Title = title;
            Author = author;
            IsAvailable = true;
        }

        public void Borrow()
        {
            if (!IsAvailable) 
                throw new InvalidOperationException("Book is already Borrowed.");

            IsAvailable = false;
        }

        public void Return()
        {
            IsAvailable = true;
        }

    }
}
