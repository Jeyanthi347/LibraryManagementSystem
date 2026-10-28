using System;

namespace LibraryManagementSystem.Models
{
    public class BookViewRow
    {
        public int BookId { get; set; }

        public string Title { get; set; }

        public string AuthorName { get; set; }

        public string CategoryName { get; set; }

        public int TotalCopies { get; set; }

        public int AvailableCopies { get; set; }

        public bool IsActive { get; set; }

        public BookViewRow()
        {
            Title = string.Empty;
            AuthorName = string.Empty;
            CategoryName = string.Empty;
        }
    }

    public class BorrowViewRow
    {
        public int BorrowId { get; set; }

        public string BookTitle { get; set; }

        public string MemberName { get; set; }

        public DateTime BorrowDate { get; set; }

        public DateTime DueDate { get; set; }

        public DateTime? ReturnDate { get; set; }

        public BorrowViewRow()
        {
            BookTitle = string.Empty;
            MemberName = string.Empty;
        }
    }

    public class FineViewRow
    {
        public int FineId { get; set; }

        public int BorrowId { get; set; }

        public string MemberName { get; set; }

        public string BookTitle { get; set; }

        public int OverdueDays { get; set; }

        public decimal DailyRate { get; set; }

        public decimal FineAmount { get; set; }

        public decimal AmountPaid { get; set; }

        public string Status { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? LastPaymentAt { get; set; }

        public FineViewRow()
        {
            MemberName = string.Empty;
            BookTitle = string.Empty;
            Status = "Unpaid";
        }
    }
}