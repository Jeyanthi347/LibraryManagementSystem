using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryManagementSystem.Models
{
    public class Author
    {
        public int AuthorId { get; set; }

        public string FullName { get; set; }

        public string? Country { get; set; }

        // Default constructor
        public Author()
        {
            FullName = string.Empty;
            Country = null;
        }

        // Parameterized constructor
        public Author(int authorId, string fullName, string? country)
        {
            AuthorId = authorId;
            FullName = fullName;
            Country = country;
        }
    }
}