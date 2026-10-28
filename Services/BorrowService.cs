using LibraryManagementSystem.Helpers;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Repositories;
using System;
using System.Collections.Generic;

namespace LibraryManagementSystem.Services
{
    public class BorrowService
    {
        private readonly BorrowRepository borrowRepository;

        public BorrowService()
        {
            borrowRepository = new BorrowRepository();
        }

        // Get all borrow records
        public List<BorrowViewRow> GetAllBorrows()
        {
            return borrowRepository.GetAll();
        }

        // Get a borrow record by ID
        public Borrow? GetBorrowById(int borrowId)
        {
            if (borrowId <= 0)
            {
                throw new ArgumentException(
                    "Borrow ID must be greater than zero.");
            }

            return borrowRepository.GetById(borrowId);
        }

        // Borrow a book
        public int BorrowBook(int bookId, int memberId)
        {
            if (bookId <= 0)
            {
                throw new ArgumentException(
                    "Book ID must be greater than zero.");
            }

            if (memberId <= 0)
            {
                throw new ArgumentException(
                    "Member ID must be greater than zero.");
            }

            return borrowRepository.BorrowBook(bookId, memberId);
        }

        // Return a borrowed book
        public void ReturnBook(int borrowId, DateTime returnDate)
        {
            if (borrowId <= 0)
            {
                throw new ArgumentException(
                    "Borrow ID must be greater than zero.");
            }

            if (returnDate.Date > DateRules.Today())
            {
                throw new ArgumentException(
                    "Return date cannot be in the future.");
            }

            Borrow? borrow = borrowRepository.GetById(borrowId);

            if (borrow == null)
            {
                throw new InvalidOperationException(
                    "Borrow record not found.");
            }

            if (borrow.ReturnDate.HasValue)
            {
                throw new InvalidOperationException(
                    "This book has already been returned.");
            }

            if (returnDate.Date < borrow.BorrowDate.Date)
            {
                throw new ArgumentException(
                    "Return date cannot be before the borrow date.");
            }

            borrowRepository.ReturnBook(
                    borrowId,
                    returnDate);
        }
    }
}