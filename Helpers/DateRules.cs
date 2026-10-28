using System;

namespace LibraryManagementSystem.Helpers
{
    public static class DateRules
    {
        // Get the current date and time
        public static DateTime Now()
        {
            return DateTime.Now;
        }

        // Get today's date without the time
        public static DateTime Today()
        {
            return DateTime.Today;
        }

        // Remove seconds and milliseconds
        public static DateTime TrimSeconds(DateTime dateTime)
        {
            return new DateTime(
                dateTime.Year,
                dateTime.Month,
                dateTime.Day,
                dateTime.Hour,
                dateTime.Minute,
                0
            );
        }

        // Get the end of a particular day
        public static DateTime EndOfDay(DateTime date)
        {
            return date.Date.AddDays(1).AddSeconds(-1);
        }

        // Calculate the due date based on the borrowing date
        public static DateTime DefaultDueDate(DateTime borrowDate)
        {
            return EndOfDay(
                borrowDate.Date.AddDays(LibrarySettings.LoanPeriodDays)
            );
        }

        // Check whether a book is overdue
        public static bool IsOverdue(DateTime dueDate, DateTime comparisonDate)
        {
            return comparisonDate.Date > dueDate.Date;
        }

        // Calculate the number of overdue calendar days
        public static int CalculateOverdueDays(
            DateTime dueDate,
            DateTime comparisonDate)
        {
            int overdueDays = (comparisonDate.Date - dueDate.Date).Days;

            if (overdueDays < 0)
            {
                return 0;
            }

            return overdueDays;
        }

        // Calculate the fine amount
        public static decimal CalculateFine(int overdueDays)
        {
            if (overdueDays <= 0)
            {
                return 0;
            }

            return overdueDays * LibrarySettings.DailyFineRate;
        }
    }
}