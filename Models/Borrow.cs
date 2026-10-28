using System;

namespace LibraryManagementSystem.Models
{
    public class Borrow
    {
        public int BorrowId { get; set; }

        public int BookId { get; set; }

        public int MemberId { get; set; }

        public DateTime BorrowDate { get; set; }

        public DateTime DueDate { get; set; }

        public DateTime? ReturnDate { get; set; }

        // Default constructor
        public Borrow()
        {
            BorrowDate = DateTime.Now;
            DueDate = BorrowDate.Date.AddDays(14).AddHours(23).AddMinutes(59).AddSeconds(59);
            ReturnDate = null;
        }

        // Parameterized constructor
        public Borrow(
            int borrowId,
            int bookId,
            int memberId,
            DateTime borrowDate,
            DateTime dueDate,
            DateTime? returnDate)
        {
            BorrowId = borrowId;
            BookId = bookId;
            MemberId = memberId;
            BorrowDate = borrowDate;
            DueDate = dueDate;
            ReturnDate = returnDate;
        }
    }
}