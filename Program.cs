using System;
using LibraryManagementSystem.Models;

namespace LibraryManagementSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("==================================");
            Console.WriteLine(" LIBRARY MODEL TESTING");
            Console.WriteLine("==================================");

            // Test Author model
            Author author = new Author(1, "R. K. Narayan", "India");

            Console.WriteLine("\n--- Author Model ---");
            Console.WriteLine($"Author ID: {author.AuthorId}");
            Console.WriteLine($"Full Name: {author.FullName}");
            Console.WriteLine($"Country: {author.Country}");

            // Test Category model
            Category category = new Category(
                1, "Fiction", "Fictional books");

            Console.WriteLine("\n--- Category Model ---");
            Console.WriteLine($"Category ID: {category.CategoryId}");
            Console.WriteLine($"Category Name: {category.CategoryName}");
            Console.WriteLine($"Description: {category.Description}");

            // Test Book model
            Book book = new Book(
                1, "Malgudi Days", "9780140186258",
                1, 1, 1943, 5, 4, true);

            Console.WriteLine("\n--- Book Model ---");
            Console.WriteLine($"Book ID: {book.BookId}");
            Console.WriteLine($"Title: {book.Title}");
            Console.WriteLine($"ISBN: {book.Isbn}");
            Console.WriteLine($"Author ID: {book.AuthorId}");
            Console.WriteLine($"Category ID: {book.CategoryId}");
            Console.WriteLine($"Publication Year: {book.PublicationYear}");
            Console.WriteLine($"Total Copies: {book.TotalCopies}");
            Console.WriteLine($"Available Copies: {book.AvailableCopies}");
            Console.WriteLine($"Active: {book.IsActive}");

            // Test Member model
            Member member = new Member(
                1, "Arun Kumar", "arun@example.com", "9876543210",
                new DateOnly(2026, 1, 1),
                new DateOnly(2027, 1, 1), true);

            Console.WriteLine("\n--- Member Model ---");
            Console.WriteLine($"Member ID: {member.MemberId}");
            Console.WriteLine($"Full Name: {member.FullName}");
            Console.WriteLine($"Email: {member.Email}");
            Console.WriteLine($"Phone: {member.Phone}");
            Console.WriteLine($"Membership Start: {member.MembershipStartDate}");
            Console.WriteLine($"Membership Expiry: {member.MembershipExpiryDate}");
            Console.WriteLine($"Active: {member.IsActive}");

            // Test Borrow model
            DateTime borrowDate = new DateTime(2026, 10, 1, 10, 0, 0);
            DateTime dueDate = new DateTime(2026, 10, 15, 23, 59, 59);

            Borrow borrow = new Borrow(
                1, 1, 1, borrowDate, dueDate, null);

            Console.WriteLine("\n--- Borrow Model ---");
            Console.WriteLine($"Borrow ID: {borrow.BorrowId}");
            Console.WriteLine($"Book ID: {borrow.BookId}");
            Console.WriteLine($"Member ID: {borrow.MemberId}");
            Console.WriteLine($"Borrow Date: {borrow.BorrowDate}");
            Console.WriteLine($"Due Date: {borrow.DueDate}");
            Console.WriteLine($"Return Date: {borrow.ReturnDate?.ToString() ?? "Not returned"}");

            // Test Fine model
            Fine fine = new Fine(
                1, 1, 3, 5.00m, 15.00m, 0.00m,
                "Unpaid", DateTime.Now, null);

            Console.WriteLine("\n--- Fine Model ---");
            Console.WriteLine($"Fine ID: {fine.FineId}");
            Console.WriteLine($"Borrow ID: {fine.BorrowId}");
            Console.WriteLine($"Overdue Days: {fine.OverdueDays}");
            Console.WriteLine($"Daily Rate: {fine.DailyRate}");
            Console.WriteLine($"Fine Amount: {fine.FineAmount}");
            Console.WriteLine($"Amount Paid: {fine.AmountPaid}");
            Console.WriteLine($"Status: {fine.Status}");
            Console.WriteLine($"Created At: {fine.CreatedAt}");
            Console.WriteLine($"Last Payment: {fine.LastPaymentAt?.ToString() ?? "No payment yet"}");

            // Test BookViewRow
            BookViewRow bookRow = new BookViewRow
            {
                BookId = 1,
                Title = "Malgudi Days",
                Isbn = "9780140186258",
                AuthorName = "R. K. Narayan",
                CategoryName = "Fiction",
                TotalCopies = 5,
                AvailableCopies = 4
            };

            Console.WriteLine("\n--- Book View Row ---");
            Console.WriteLine($"Title: {bookRow.Title}");
            Console.WriteLine($"Author: {bookRow.AuthorName}");
            Console.WriteLine($"Category: {bookRow.CategoryName}");

            // Test BorrowViewRow
            BorrowViewRow borrowRow = new BorrowViewRow
            {
                BorrowId = 1,
                BookTitle = "Malgudi Days",
                MemberName = "Arun Kumar",
                BorrowDate = borrowDate,
                DueDate = dueDate,
                ReturnDate = null
            };

            Console.WriteLine("\n--- Borrow View Row ---");
            Console.WriteLine($"Borrow ID: {borrowRow.BorrowId}");
            Console.WriteLine($"Book: {borrowRow.BookTitle}");
            Console.WriteLine($"Member: {borrowRow.MemberName}");

            // Test FineViewRow
            FineViewRow fineRow = new FineViewRow
            {
                FineId = 1,
                MemberName = "Arun Kumar",
                BookTitle = "Malgudi Days",
                OverdueDays = 3,
                FineAmount = 15.00m,
                AmountPaid = 0.00m,
                Status = "Unpaid"
            };

            Console.WriteLine("\n--- Fine View Row ---");
            Console.WriteLine($"Member: {fineRow.MemberName}");
            Console.WriteLine($"Book: {fineRow.BookTitle}");
            Console.WriteLine($"Fine Amount: {fineRow.FineAmount}");
            Console.WriteLine($"Status: {fineRow.Status}");

            Console.WriteLine("\n==================================");
            Console.WriteLine(" ALL MODEL TESTS COMPLETED");
            Console.WriteLine("==================================");

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
    }
}