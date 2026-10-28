using System;

namespace LibraryManagementSystem.Models
{
    public class Fine
    {
        public int FineId { get; set; }

        public int BorrowId { get; set; }

        public int OverdueDays { get; set; }

        public decimal DailyRate { get; set; }

        public decimal FineAmount { get; set; }

        public decimal AmountPaid { get; set; }

        public string Status { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? LastPaymentAt { get; set; }

        // Default constructor
        public Fine()
        {
            Status = "Unpaid";
            AmountPaid = 0;
            CreatedAt = DateTime.Now;
            LastPaymentAt = null;
        }

        // Parameterized constructor
        public Fine(
            int fineId,
            int borrowId,
            int overdueDays,
            decimal dailyRate,
            decimal fineAmount,
            decimal amountPaid,
            string status,
            DateTime createdAt,
            DateTime? lastPaymentAt)
        {
            FineId = fineId;
            BorrowId = borrowId;
            OverdueDays = overdueDays;
            DailyRate = dailyRate;
            FineAmount = fineAmount;
            AmountPaid = amountPaid;
            Status = status;
            CreatedAt = createdAt;
            LastPaymentAt = lastPaymentAt;
        }
    }
}