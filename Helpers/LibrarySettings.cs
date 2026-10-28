namespace LibraryManagementSystem.Helpers
{
    public static class LibrarySettings
    {
        // Number of days a member can borrow a book
        public const int LoanPeriodDays = 14;

        // Maximum number of books a member can borrow at once
        public const int MaxActiveBorrows = 3;

        // Fine charged for each overdue day
        public const decimal DailyFineRate = 5.00m;
    }
}