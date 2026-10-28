using LibraryManagementSystem.Models;
using LibraryManagementSystem.Repositories;
using System;
using System.Collections.Generic;

namespace LibraryManagementSystem.Services
{
    public class BookService
    {
        private readonly BookRepository bookRepository;

        public BookService()
        {
            bookRepository = new BookRepository();
        }

        // Get all active books
        public List<BookViewRow> GetAllBooks()
        {
            return bookRepository.GetAll();
        }

        // Get a book by ID
        public Book? GetBookById(int bookId)
        {
            if (bookId <= 0)
            {
                throw new ArgumentException(
                    "Book ID must be greater than zero.");
            }

            return bookRepository.GetById(bookId);
        }

        // Add a new book
        public int AddBook(Book book)
        {
            if (book == null)
            {
                throw new ArgumentNullException(nameof(book));
            }

            ValidateBook(book);

            if (book.TotalCopies <= 0)
            {
                throw new ArgumentException(
                    "Total copies must be greater than zero.");
            }

            book.AvailableCopies = book.TotalCopies;

            return bookRepository.Add(book);
        }

        // Update an existing book
        public bool UpdateBook(Book book)
        {
            if (book == null)
            {
                throw new ArgumentNullException(nameof(book));
            }

            if (book.BookId <= 0)
            {
                throw new ArgumentException(
                    "Book ID must be greater than zero.");
            }

            ValidateBook(book);

            if (book.TotalCopies <= 0)
            {
                throw new ArgumentException(
                    "Total copies must be greater than zero.");
            }

            if (bookRepository.GetById(book.BookId) == null)
            {
                throw new InvalidOperationException(
                    "Book not found.");
            }

            return bookRepository.Update(book);
        }

        // Delete a book or deactivate it if it has borrow history
        public bool DeleteBook(int bookId)
        {
            if (bookId <= 0)
            {
                throw new ArgumentException(
                    "Book ID must be greater than zero.");
            }

            if (bookRepository.GetById(bookId) == null)
            {
                throw new InvalidOperationException(
                    "Book not found.");
            }

            return bookRepository.Delete(bookId);
        }

        // Validate book details
        private void ValidateBook(Book book)
        {
            book.Title = book.Title.Trim();

            if (string.IsNullOrWhiteSpace(book.Title))
            {
                throw new ArgumentException(
                    "Book title cannot be empty.");
            }

            if (book.Title.Length > 200)
            {
                throw new ArgumentException(
                    "Book title cannot exceed 200 characters.");
            }

            if (book.Isbn != null)
            {
                book.Isbn = book.Isbn.Trim();

                if (book.Isbn.Length == 0)
                {
                    book.Isbn = null;
                }
            }

            if (book.AuthorId <= 0)
            {
                throw new ArgumentException(
                    "Please select a valid author.");
            }

            if (book.CategoryId <= 0)
            {
                throw new ArgumentException(
                    "Please select a valid category.");
            }

            if (book.PublicationYear.HasValue)
            {
                int currentYear = DateTime.Today.Year;

                if (book.PublicationYear.Value < 1 ||
                    book.PublicationYear.Value > currentYear)
                {
                    throw new ArgumentException(
                        "Publication year is invalid.");
                }
            }
        }
    }
}