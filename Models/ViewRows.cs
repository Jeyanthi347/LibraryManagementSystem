namespace LibraryManagementSystem.Models
{
    public class BookViewRow
    {
        public int BookId { get; set; }

        public string Title { get; set; }

        public string Isbn { get; set; }

        public string AuthorName { get; set; }

        public string CategoryName { get; set; }

        public int TotalCopies { get; set; }

        public int AvailableCopies { get; set; }

        public BookViewRow()
        {
            Title = string.Empty;
            Isbn = string.Empty;
            AuthorName = string.Empty;
            CategoryName = string.Empty;
        }
    }

    public class BorrowViewRow
    {
        public int BorrowId { get; set; }

        public string BookTitle { get; set; }

        public string MemberName { get; set; }

        public System.DateTime BorrowDate { get; set; }

        public System.DateTime DueDate { get; set; }

        public System.DateTime? ReturnDate { get; set; }

        public BorrowViewRow()
        {
            BookTitle = string.Empty;
            MemberName = string.Empty;
        }
    }

    public class FineViewRow
    {
        public int FineId { get; set; }

        public string MemberName { get; set; }

        public string BookTitle { get; set; }

        public int OverdueDays { get; set; }

        public decimal FineAmount { get; set; }

        public decimal AmountPaid { get; set; }

        public string Status { get; set; }

        public FineViewRow()
        {
            MemberName = string.Empty;
            BookTitle = string.Empty;
            Status = "Unpaid";
        }
    }
}