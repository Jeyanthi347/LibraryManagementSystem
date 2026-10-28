namespace LibraryManagementSystem.Models
{
    public class Book
    {
        public int BookId { get; set; }

        public string Title { get; set; }

        public string Isbn { get; set; }

        public int AuthorId { get; set; }

        public int CategoryId { get; set; }

        public int? PublicationYear { get; set; }

        public int TotalCopies { get; set; }

        public int AvailableCopies { get; set; }

        public bool IsActive { get; set; }

        // Default constructor
        public Book()
        {
            Title = string.Empty;
            Isbn = string.Empty;
            TotalCopies = 1;
            AvailableCopies = 1;
            IsActive = true;
        }

        // Parameterized constructor
        public Book(
            int bookId,
            string title,
            string isbn,
            int authorId,
            int categoryId,
            int? publicationYear,
            int totalCopies,
            int availableCopies,
            bool isActive)
        {
            BookId = bookId;
            Title = title;
            Isbn = isbn;
            AuthorId = authorId;
            CategoryId = categoryId;
            PublicationYear = publicationYear;
            TotalCopies = totalCopies;
            AvailableCopies = availableCopies;
            IsActive = isActive;
        }
    }
}