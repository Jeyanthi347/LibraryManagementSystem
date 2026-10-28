
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Services;
using System;
using System.Collections.Generic;

namespace LibraryManagementSystem
{
    public class Program
    {
        static AuthorService authorService = new AuthorService();
        static CategoryService categoryService = new CategoryService();
        static BookService bookService = new BookService();
        static MemberService memberService = new MemberService();
        static BorrowService borrowService = new BorrowService();
        static FineService fineService = new FineService();
        static ReportService reportService = new ReportService();

        public static void Main(string[] args)
        {
            bool running = true;

            while (running)
            {
                Console.Clear();
                Console.WriteLine("==================================");
                Console.WriteLine("     LIBRARY MANAGEMENT SYSTEM");
                Console.WriteLine("==================================");
                Console.WriteLine("1. Manage Authors");
                Console.WriteLine("2. Manage Categories");
                Console.WriteLine("3. Manage Books");
                Console.WriteLine("4. Manage Members");
                Console.WriteLine("5. Borrow a Book");
                Console.WriteLine("6. Return a Book");
                Console.WriteLine("7. Manage Fines");
                Console.WriteLine("8. Reports");
                Console.WriteLine("0. Exit");
                Console.WriteLine("==================================");

                int choice = ReadInt("Enter your choice: ");

                try
                {
                    switch (choice)
                    {
                        case 1:
                            ManageAuthors();
                            break;

                        case 2:
                            ManageCategories();
                            break;

                        case 3:
                            ManageBooks();
                            break;

                        case 4:
                            ManageMembers();
                            break;

                        case 5:
                            BorrowBook();
                            break;

                        case 6:
                            ReturnBook();
                            break;

                        case 7:
                            ManageFines();
                            break;

                        case 8:
                            ShowReports();
                            break;

                        case 0:
                            running = false;
                            Console.WriteLine(
                                "Thank you for using the Library Management System.");
                            break;

                        default:
                            Console.WriteLine("Invalid choice. Please try again.");
                            Pause();
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("\nError: " + ex.Message);
                    Pause();
                }
            }
        }

        // Author menu
        static void ManageAuthors()
        {
            bool back = false;

            while (!back)
            {
                Console.Clear();
                Console.WriteLine("========== AUTHOR MANAGEMENT ==========");
                Console.WriteLine("1. View All Authors");
                Console.WriteLine("2. Add Author");
                Console.WriteLine("3. Update Author");
                Console.WriteLine("4. Delete Author");
                Console.WriteLine("0. Back to Main Menu");

                int choice = ReadInt("Enter your choice: ");

                try
                {
                    switch (choice)
                    {
                        case 1:
                            {
                                List<Author> authors =
                                    authorService.GetAllAuthors();

                                if (authors.Count == 0)
                                {
                                    Console.WriteLine("No authors found.");
                                }
                                else
                                {
                                    foreach (Author author in authors)
                                    {
                                        Console.WriteLine(
                                            $"ID: {author.AuthorId} | " +
                                            $"Name: {author.FullName} | " +
                                            $"Country: {author.Country}");
                                    }
                                }

                                Pause();
                                break;
                            }

                        case 2:
                            {
                                Author newAuthor = new Author();

                                newAuthor.FullName =
                                    ReadText("Enter author name: ");

                                newAuthor.Country =
                                    ReadOptionalText(
                                        "Enter country (optional): ");

                                int authorId =
                                    authorService.AddAuthor(newAuthor);

                                Console.WriteLine(
                                    $"Author added successfully. ID: {authorId}");

                                Pause();
                                break;
                            }

                        case 3:
                            {
                                int updateAuthorId =
                                    ReadInt("Enter author ID: ");

                                Author? authorToUpdate =
                                    authorService.GetAuthorById(updateAuthorId);

                                if (authorToUpdate == null)
                                {
                                    Console.WriteLine("Author not found.");
                                }
                                else
                                {
                                    authorToUpdate.FullName =
                                        ReadText("Enter new author name: ");

                                    authorToUpdate.Country =
                                        ReadOptionalText(
                                            "Enter country (optional): ");

                                    authorService.UpdateAuthor(authorToUpdate);

                                    Console.WriteLine(
                                        "Author updated successfully.");
                                }

                                Pause();
                                break;
                            }

                        case 4:
                            {
                                int deleteAuthorId =
                                    ReadInt("Enter author ID to delete: ");

                                Author? authorToDelete =
                                    authorService.GetAuthorById(deleteAuthorId);

                                if (authorToDelete == null)
                                {
                                    Console.WriteLine("Author not found.");
                                }
                                else
                                {
                                    authorService.DeleteAuthor(deleteAuthorId);

                                    Console.WriteLine(
                                        "Author deletion processed.");
                                }

                                Pause();
                                break;
                            }

                        case 0:
                            back = true;
                            break;

                        default:
                            Console.WriteLine("Invalid choice.");
                            Pause();
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                    Pause();
                }
            }
        }

        // Category menu
        static void ManageCategories()
        {
            bool back = false;

            while (!back)
            {
                Console.Clear();
                Console.WriteLine("========== CATEGORY MANAGEMENT ==========");
                Console.WriteLine("1. View All Categories");
                Console.WriteLine("2. Add Category");
                Console.WriteLine("3. Update Category");
                Console.WriteLine("4. Delete Category");
                Console.WriteLine("0. Back to Main Menu");

                int choice = ReadInt("Enter your choice: ");

                try
                {
                    switch (choice)
                    {
                        case 1:
                            {
                                List<Category> categories =
                                    categoryService.GetAllCategories();

                                if (categories.Count == 0)
                                {
                                    Console.WriteLine("No categories found.");
                                }
                                else
                                {
                                    foreach (Category category in categories)
                                    {
                                        Console.WriteLine(
                                            $"ID: {category.CategoryId} | " +
                                            $"Name: {category.CategoryName} | " +
                                            $"Description: {category.Description}");
                                    }
                                }

                                Pause();
                                break;
                            }

                        case 2:
                            {
                                Category newCategory = new Category();

                                newCategory.CategoryName =
                                    ReadText("Enter category name: ");

                                newCategory.Description =
                                    ReadOptionalText(
                                        "Enter description (optional): ");

                                int categoryId =
                                    categoryService.AddCategory(newCategory);

                                Console.WriteLine(
                                    $"Category added successfully. ID: {categoryId}");

                                Pause();
                                break;
                            }

                        case 3:
                            {
                                int updateCategoryId =
                                    ReadInt("Enter category ID: ");

                                Category? categoryToUpdate =
                                    categoryService.GetCategoryById(
                                        updateCategoryId);

                                if (categoryToUpdate == null)
                                {
                                    Console.WriteLine("Category not found.");
                                }
                                else
                                {
                                    categoryToUpdate.CategoryName =
                                        ReadText("Enter new category name: ");

                                    categoryToUpdate.Description =
                                        ReadOptionalText(
                                            "Enter description (optional): ");

                                    categoryService.UpdateCategory(
                                        categoryToUpdate);

                                    Console.WriteLine(
                                        "Category updated successfully.");
                                }

                                Pause();
                                break;
                            }

                        case 4:
                            {
                                int deleteCategoryId =
                                    ReadInt("Enter category ID to delete: ");

                                Category? categoryToDelete =
                                    categoryService.GetCategoryById(
                                        deleteCategoryId);

                                if (categoryToDelete == null)
                                {
                                    Console.WriteLine("Category not found.");
                                }
                                else
                                {
                                    categoryService.DeleteCategory(
                                        deleteCategoryId);

                                    Console.WriteLine(
                                        "Category deletion processed.");
                                }

                                Pause();
                                break;
                            }

                        case 0:
                            back = true;
                            break;

                        default:
                            Console.WriteLine("Invalid choice.");
                            Pause();
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                    Pause();
                }
            }
        }

        // Book menu
        static void ManageBooks()
        {
            bool back = false;

            while (!back)
            {
                Console.Clear();
                Console.WriteLine("========== BOOK MANAGEMENT ==========");
                Console.WriteLine("1. View All Books");
                Console.WriteLine("2. Add Book");
                Console.WriteLine("3. Update Book");
                Console.WriteLine("4. Delete or Deactivate Book");
                Console.WriteLine("0. Back to Main Menu");

                int choice = ReadInt("Enter your choice: ");

                try
                {
                    switch (choice)
                    {
                        case 1:
                            {
                                List<BookViewRow> books =
                                    bookService.GetAllBooks();

                                if (books.Count == 0)
                                {
                                    Console.WriteLine("No books found.");
                                }
                                else
                                {
                                    foreach (BookViewRow book in books)
                                    {
                                        Console.WriteLine(
                                            $"ID: {book.BookId} | " +
                                            $"Title: {book.Title} | " +
                                            $"ISBN: {book.Isbn} | " +
                                            $"Author: {book.AuthorName} | " +
                                            $"Category: {book.CategoryName} | " +
                                            $"Total: {book.TotalCopies} | " +
                                            $"Available: {book.AvailableCopies}");
                                    }
                                }

                                Pause();
                                break;
                            }

                        case 2:
                            {
                                Book newBook = new Book();

                                newBook.Title =
                                    ReadText("Enter book title: ");

                                newBook.Isbn =
                                    ReadOptionalText("Enter ISBN (optional): ");

                                newBook.AuthorId =
                                    ReadInt("Enter author ID: ");

                                newBook.CategoryId =
                                    ReadInt("Enter category ID: ");

                                string yearText =
                                    ReadOptionalText(
                                        "Enter publication year (optional): ") ?? "";

                                if (yearText != "")
                                {
                                    if (int.TryParse(yearText, out int year))
                                    {
                                        newBook.PublicationYear = year;
                                    }
                                    else
                                    {
                                        throw new ArgumentException(
                                            "Publication year must be a number.");
                                    }
                                }

                                newBook.TotalCopies =
                                    ReadInt("Enter total copies: ");

                                int bookId = bookService.AddBook(newBook);

                                Console.WriteLine(
                                    $"Book added successfully. ID: {bookId}");

                                Pause();
                                break;
                            }

                        case 3:
                            {
                                int updateBookId =
                                    ReadInt("Enter book ID: ");

                                Book? bookToUpdate =
                                    bookService.GetBookById(updateBookId);

                                if (bookToUpdate == null)
                                {
                                    Console.WriteLine("Book not found.");
                                }
                                else
                                {
                                    bookToUpdate.Title =
                                        ReadText("Enter new title: ");

                                    bookToUpdate.Isbn =
                                        ReadOptionalText(
                                            "Enter ISBN (optional): ");

                                    bookToUpdate.AuthorId =
                                        ReadInt("Enter author ID: ");

                                    bookToUpdate.CategoryId =
                                        ReadInt("Enter category ID: ");

                                    string newYearText =
                                        ReadOptionalText(
                                            "Enter publication year (optional): ") ?? "";

                                    if (newYearText == "")
                                    {
                                        bookToUpdate.PublicationYear = null;
                                    }
                                    else if (int.TryParse(
                                        newYearText, out int newYear))
                                    {
                                        bookToUpdate.PublicationYear = newYear;
                                    }
                                    else
                                    {
                                        throw new ArgumentException(
                                            "Publication year must be a number.");
                                    }

                                    bookToUpdate.TotalCopies =
                                        ReadInt("Enter total copies: ");

                                    bookService.UpdateBook(bookToUpdate);

                                    Console.WriteLine(
                                        "Book updated successfully.");
                                }

                                Pause();
                                break;
                            }

                        case 4:
                            {
                                int deleteBookId =
                                    ReadInt("Enter book ID: ");

                                Book? bookToDelete =
                                    bookService.GetBookById(deleteBookId);

                                if (bookToDelete == null)
                                {
                                    Console.WriteLine("Book not found.");
                                }
                                else
                                {
                                    bookService.DeleteBook(deleteBookId);

                                    Console.WriteLine(
                                        "Book deletion/deactivation processed.");
                                }

                                Pause();
                                break;
                            }

                        case 0:
                            back = true;
                            break;

                        default:
                            Console.WriteLine("Invalid choice.");
                            Pause();
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                    Pause();
                }
            }
        }

        // Member menu
        static void ManageMembers()
        {
            bool back = false;

            while (!back)
            {
                Console.Clear();
                Console.WriteLine("========== MEMBER MANAGEMENT ==========");
                Console.WriteLine("1. View All Members");
                Console.WriteLine("2. Add Member");
                Console.WriteLine("3. Update Member");
                Console.WriteLine("4. Deactivate Member");
                Console.WriteLine("0. Back to Main Menu");

                int choice = ReadInt("Enter your choice: ");

                try
                {
                    switch (choice)
                    {
                        case 1:
                            {
                                List<Member> members =
                                    memberService.GetAllMembers();

                                if (members.Count == 0)
                                {
                                    Console.WriteLine("No members found.");
                                }
                                else
                                {
                                    foreach (Member member in members)
                                    {
                                        Console.WriteLine(
                                            $"ID: {member.MemberId} | " +
                                            $"Name: {member.FullName} | " +
                                            $"Email: {member.Email} | " +
                                            $"Phone: {member.Phone} | " +
                                            $"Start: {member.MembershipStartDate} | " +
                                            $"Expiry: {member.MembershipExpiryDate} | " +
                                            $"Active: {member.IsActive}");
                                    }
                                }

                                Pause();
                                break;
                            }

                        case 2:
                            {
                                Member newMember = new Member();

                                newMember.FullName =
                                    ReadText("Enter member name: ");

                                newMember.Email =
                                    ReadText("Enter email: ");

                                newMember.Phone =
                                    ReadText("Enter 10-digit phone number: ");

                                newMember.MembershipStartDate =
                                    ReadDateOnly(
                                        "Enter membership start date (yyyy-MM-dd): ");

                                newMember.MembershipExpiryDate =
                                    ReadDateOnly(
                                        "Enter membership expiry date (yyyy-MM-dd): ");

                                int memberId =
                                    memberService.AddMember(newMember);

                                Console.WriteLine(
                                    $"Member added successfully. ID: {memberId}");

                                Pause();
                                break;
                            }

                        case 3:
                            {
                                int updateMemberId =
                                    ReadInt("Enter member ID: ");

                                Member? memberToUpdate =
                                    memberService.GetMemberById(updateMemberId);

                                if (memberToUpdate == null)
                                {
                                    Console.WriteLine("Member not found.");
                                }
                                else
                                {
                                    memberToUpdate.FullName =
                                        ReadText("Enter new name: ");

                                    memberToUpdate.Email =
                                        ReadText("Enter new email: ");

                                    memberToUpdate.Phone =
                                        ReadText("Enter 10-digit phone number: ");

                                    memberToUpdate.MembershipStartDate =
                                        ReadDateOnly(
                                            "Enter start date (yyyy-MM-dd): ");

                                    memberToUpdate.MembershipExpiryDate =
                                        ReadDateOnly(
                                            "Enter expiry date (yyyy-MM-dd): ");

                                    memberService.UpdateMember(memberToUpdate);

                                    Console.WriteLine(
                                        "Member updated successfully.");
                                }

                                Pause();
                                break;
                            }

                        case 4:
                            {
                                int deactivateMemberId =
                                    ReadInt("Enter member ID to deactivate: ");

                                Member? memberToDeactivate =
                                    memberService.GetMemberById(
                                        deactivateMemberId);

                                if (memberToDeactivate == null)
                                {
                                    Console.WriteLine("Member not found.");
                                }
                                else
                                {
                                    memberService.DeactivateMember(
                                        deactivateMemberId);

                                    Console.WriteLine(
                                        "Member deactivation processed.");
                                }

                                Pause();
                                break;
                            }

                        case 0:
                            back = true;
                            break;

                        default:
                            Console.WriteLine("Invalid choice.");
                            Pause();
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                    Pause();
                }
            }
        }

        // Borrow a book
        static void BorrowBook()
        {
            Console.Clear();
            Console.WriteLine("========== BORROW A BOOK ==========");

            int bookId = ReadInt("Enter book ID: ");
            int memberId = ReadInt("Enter member ID: ");

            int borrowId = borrowService.BorrowBook(bookId, memberId);

            Console.WriteLine(
                $"Book borrowed successfully. Borrow ID: {borrowId}");

            Pause();
        }

        // Return a book
        static void ReturnBook()
        {
            Console.Clear();
            Console.WriteLine("========== RETURN A BOOK ==========");

            int borrowId = ReadInt("Enter borrow ID: ");
            DateTime returnDate = DateTime.Now;

            borrowService.ReturnBook(borrowId, returnDate);

            Console.WriteLine("Book returned successfully.");
            Console.WriteLine(
                "If the book was overdue, a fine has been calculated.");

            Pause();
        }

        // Fine menu
        static void ManageFines()
        {
            bool back = false;

            while (!back)
            {
                Console.Clear();
                Console.WriteLine("========== FINE MANAGEMENT ==========");
                Console.WriteLine("1. View All Fines");
                Console.WriteLine("2. Pay a Fine");
                Console.WriteLine("0. Back to Main Menu");

                int choice = ReadInt("Enter your choice: ");

                try
                {
                    switch (choice)
                    {
                        case 1:
                            {
                                List<FineViewRow> fines =
                                    fineService.GetAllFines();

                                if (fines.Count == 0)
                                {
                                    Console.WriteLine("No fines found.");
                                }
                                else
                                {
                                    foreach (FineViewRow fine in fines)
                                    {
                                        Console.WriteLine(
                                            $"Fine ID: {fine.FineId} | " +
                                            $"Borrow ID: {fine.BorrowId} | " +
                                            $"Member: {fine.MemberName} | " +
                                            $"Book: {fine.BookTitle} | " +
                                            $"Days: {fine.OverdueDays} | " +
                                            $"Fine: {fine.FineAmount:C} | " +
                                            $"Paid: {fine.AmountPaid:C} | " +
                                            $"Status: {fine.Status}");
                                    }
                                }

                                Pause();
                                break;
                            }

                        case 2:
                            {
                                int fineId = ReadInt("Enter fine ID: ");

                                Fine? fineToPay =
                                    fineService.GetFineById(fineId);

                                if (fineToPay == null)
                                {
                                    Console.WriteLine("Fine not found.");
                                }
                                else
                                {
                                    decimal amount =
                                        ReadDecimal("Enter payment amount: ");

                                    fineService.PayFine(fineId, amount);

                                    Console.WriteLine(
                                        "Fine payment processed successfully.");
                                }

                                Pause();
                                break;
                            }

                        case 0:
                            back = true;
                            break;

                        default:
                            Console.WriteLine("Invalid choice.");
                            Pause();
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                    Pause();
                }
            }
        }

        // Reports menu
        static void ShowReports()
        {
            bool back = false;

            while (!back)
            {
                Console.Clear();
                Console.WriteLine("========== REPORTS ==========");
                Console.WriteLine("1. Inventory Report");
                Console.WriteLine("2. Borrow History Report");
                Console.WriteLine("3. Overdue Books Report");
                Console.WriteLine("4. Fine Report");
                Console.WriteLine("0. Back to Main Menu");

                int choice = ReadInt("Enter your choice: ");

                try
                {
                    switch (choice)
                    {
                        case 1:
                            {
                                List<BookViewRow> books =
                                    reportService.GetInventoryReport();

                                if (books.Count == 0)
                                {
                                    Console.WriteLine(
                                        "No inventory records found.");
                                }
                                else
                                {
                                    foreach (BookViewRow book in books)
                                    {
                                        Console.WriteLine(
                                            $"ID: {book.BookId} | " +
                                            $"Title: {book.Title} | " +
                                            $"Author: {book.AuthorName} | " +
                                            $"Category: {book.CategoryName} | " +
                                            $"Total: {book.TotalCopies} | " +
                                            $"Available: {book.AvailableCopies} | " +
                                            $"Active: {book.IsActive}");
                                    }
                                }

                                Pause();
                                break;
                            }

                        case 2:
                            {
                                List<BorrowViewRow> borrowHistory =
                                    reportService.GetBorrowHistoryReport();

                                if (borrowHistory.Count == 0)
                                {
                                    Console.WriteLine(
                                        "No borrow records found.");
                                }
                                else
                                {
                                    foreach (BorrowViewRow borrow in borrowHistory)
                                    {
                                        Console.WriteLine(
                                            $"Borrow ID: {borrow.BorrowId} | " +
                                            $"Book: {borrow.BookTitle} | " +
                                            $"Member: {borrow.MemberName} | " +
                                            $"Borrowed: {borrow.BorrowDate} | " +
                                            $"Due: {borrow.DueDate} | " +
                                            $"Returned: {borrow.ReturnDate}");
                                    }
                                }

                                Pause();
                                break;
                            }

                        case 3:
                            {
                                List<BorrowViewRow> overdue =
                                    reportService.GetOverdueReport();

                                if (overdue.Count == 0)
                                {
                                    Console.WriteLine(
                                        "No overdue books found.");
                                }
                                else
                                {
                                    foreach (BorrowViewRow borrow in overdue)
                                    {
                                        Console.WriteLine(
                                            $"Borrow ID: {borrow.BorrowId} | " +
                                            $"Book: {borrow.BookTitle} | " +
                                            $"Member: {borrow.MemberName} | " +
                                            $"Due: {borrow.DueDate}");
                                    }
                                }

                                Pause();
                                break;
                            }

                        case 4:
                            {
                                List<FineViewRow> fineReport =
                                    reportService.GetFineReport();

                                if (fineReport.Count == 0)
                                {
                                    Console.WriteLine(
                                        "No fine records found.");
                                }
                                else
                                {
                                    foreach (FineViewRow fine in fineReport)
                                    {
                                        Console.WriteLine(
                                            $"Fine ID: {fine.FineId} | " +
                                            $"Member: {fine.MemberName} | " +
                                            $"Book: {fine.BookTitle} | " +
                                            $"Days: {fine.OverdueDays} | " +
                                            $"Amount: {fine.FineAmount:C} | " +
                                            $"Paid: {fine.AmountPaid:C} | " +
                                            $"Status: {fine.Status}");
                                    }
                                }

                                Pause();
                                break;
                            }

                        case 0:
                            back = true;
                            break;

                        default:
                            Console.WriteLine("Invalid choice.");
                            Pause();
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                    Pause();
                }
            }
        }

        // Read an integer from the console
        static int ReadInt(string message)
        {
            while (true)
            {
                Console.Write(message);
                string input = Console.ReadLine() ?? "";

                if (int.TryParse(input, out int value))
                {
                    return value;
                }

                Console.WriteLine(
                    "Please enter a valid whole number.");
            }
        }

        // Read a decimal value from the console
        static decimal ReadDecimal(string message)
        {
            while (true)
            {
                Console.Write(message);
                string input = Console.ReadLine() ?? "";

                if (decimal.TryParse(input, out decimal value))
                {
                    return value;
                }

                Console.WriteLine(
                    "Please enter a valid amount.");
            }
        }

        // Read a required string
        static string ReadText(string message)
        {
            while (true)
            {
                Console.Write(message);
                string input = Console.ReadLine() ?? "";

                if (!string.IsNullOrWhiteSpace(input))
                {
                    return input.Trim();
                }

                Console.WriteLine(
                    "This field cannot be empty.");
            }
        }

        // Read an optional string
        static string? ReadOptionalText(string message)
        {
            Console.Write(message);
            string input = Console.ReadLine() ?? "";

            if (string.IsNullOrWhiteSpace(input))
            {
                return null;
            }

            return input.Trim();
        }

        // Read a date without a time
        static DateOnly ReadDateOnly(string message)
        {
            while (true)
            {
                Console.Write(message);
                string input = Console.ReadLine() ?? "";

                if (DateOnly.TryParse(input, out DateOnly date))
                {
                    return date;
                }

                Console.WriteLine(
                    "Invalid date. Please enter a date such as 2026-10-28.");
            }
        }

        // Pause before returning to the menu
        static void Pause()
        {
            Console.WriteLine(
                "\nPress Enter to continue...");

            Console.ReadLine();
        }
    }
}
