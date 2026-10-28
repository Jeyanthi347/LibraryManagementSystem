using LibraryManagementSystem.Helpers;
using LibraryManagementSystem.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace LibraryManagementSystem.Repositories
{
    public class BorrowRepository
    {
        // Display all borrow records
        public List<BorrowViewRow> GetAll()
        {
            List<BorrowViewRow> borrows = new List<BorrowViewRow>();

            using (SqlConnection connection = DatabaseHelper.GetConnection())
            {
                connection.Open();

                string query = @"
                    SELECT
                        b.BorrowId,
                        bk.Title AS BookTitle,
                        m.FullName AS MemberName,
                        b.BorrowDate,
                        b.DueDate,
                        b.ReturnDate
                    FROM Borrows b
                    INNER JOIN Books bk ON b.BookId = bk.BookId
                    INNER JOIN Members m ON b.MemberId = m.MemberId
                    ORDER BY b.BorrowId DESC;";

                using (SqlCommand command = new SqlCommand(query, connection))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        BorrowViewRow row = new BorrowViewRow();

                        row.BorrowId = reader.GetInt32(0);
                        row.BookTitle = reader.GetString(1);
                        row.MemberName = reader.GetString(2);
                        row.BorrowDate = reader.GetDateTime(3);
                        row.DueDate = reader.GetDateTime(4);

                        row.ReturnDate = reader.IsDBNull(5)
                            ? null
                            : reader.GetDateTime(5);

                        borrows.Add(row);
                    }
                }
            }

            return borrows;
        }

        // Get one borrow record using its ID
        public Borrow? GetById(int borrowId)
        {
            using (SqlConnection connection = DatabaseHelper.GetConnection())
            {
                connection.Open();

                string query = @"
                    SELECT BorrowId, BookId, MemberId,
                           BorrowDate, DueDate, ReturnDate
                    FROM Borrows
                    WHERE BorrowId = @BorrowId;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@BorrowId", SqlDbType.Int).Value =
                        borrowId;

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Borrow borrow = new Borrow();

                            borrow.BorrowId = reader.GetInt32(0);
                            borrow.BookId = reader.GetInt32(1);
                            borrow.MemberId = reader.GetInt32(2);
                            borrow.BorrowDate = reader.GetDateTime(3);
                            borrow.DueDate = reader.GetDateTime(4);

                            borrow.ReturnDate = reader.IsDBNull(5)
                                ? null
                                : reader.GetDateTime(5);

                            return borrow;
                        }
                    }
                }
            }

            return null;
        }

        // Borrow a book
        public int BorrowBook(int bookId, int memberId)
        {
            using (SqlConnection connection = DatabaseHelper.GetConnection())
            {
                connection.Open();

                using (SqlTransaction transaction =
                       connection.BeginTransaction(IsolationLevel.Serializable))
                {
                    try
                    {
                        // Check whether the book exists, is active,
                        // and has available copies.
                        string bookQuery = @"
                            SELECT AvailableCopies
                            FROM Books WITH (UPDLOCK, HOLDLOCK)
                            WHERE BookId = @BookId
                              AND IsActive = 1;";

                        int availableCopies;

                        using (SqlCommand command =
                               new SqlCommand(bookQuery, connection, transaction))
                        {
                            command.Parameters.Add("@BookId", SqlDbType.Int).Value =
                                bookId;

                            object? result = command.ExecuteScalar();

                            if (result == null)
                            {
                                throw new InvalidOperationException(
                                    "Book not found or the book is inactive.");
                            }

                            availableCopies = Convert.ToInt32(result);
                        }

                        if (availableCopies <= 0)
                        {
                            throw new InvalidOperationException(
                                "No copies of this book are available.");
                        }

                        // Check whether the member is active
                        // and the membership is valid today.
                        string memberQuery = @"
                            SELECT COUNT(*)
                            FROM Members WITH (UPDLOCK, HOLDLOCK)
                            WHERE MemberId = @MemberId
                              AND IsActive = 1
                              AND MembershipStartDate <= @Today
                              AND MembershipExpiryDate >= @Today;";

                        using (SqlCommand command =
                               new SqlCommand(memberQuery, connection, transaction))
                        {
                            command.Parameters.Add("@MemberId", SqlDbType.Int).Value =
                                memberId;

                            command.Parameters.Add("@Today", SqlDbType.Date).Value =
                                DateTime.Today;

                            int validMember =
                                Convert.ToInt32(command.ExecuteScalar());

                            if (validMember == 0)
                            {
                                throw new InvalidOperationException(
                                    "Member not found, inactive, or membership expired.");
                            }
                        }

                        // Check the maximum active borrowing limit
                        string countQuery = @"
                            SELECT COUNT(*)
                            FROM Borrows WITH (UPDLOCK, HOLDLOCK)
                            WHERE MemberId = @MemberId
                              AND ReturnDate IS NULL;";

                        using (SqlCommand command =
                               new SqlCommand(countQuery, connection, transaction))
                        {
                            command.Parameters.Add("@MemberId", SqlDbType.Int).Value =
                                memberId;

                            int activeBorrows =
                                Convert.ToInt32(command.ExecuteScalar());

                            if (activeBorrows >= LibrarySettings.MaxActiveBorrows)
                            {
                                throw new InvalidOperationException(
                                    "Member has reached the maximum active borrow limit.");
                            }
                        }

                        // Prevent the same member from borrowing
                        // the same book again before returning it.
                        string duplicateQuery = @"
                            SELECT COUNT(*)
                            FROM Borrows
                            WHERE MemberId = @MemberId
                              AND BookId = @BookId
                              AND ReturnDate IS NULL;";

                        using (SqlCommand command =
                               new SqlCommand(duplicateQuery, connection, transaction))
                        {
                            command.Parameters.Add("@MemberId", SqlDbType.Int).Value =
                                memberId;

                            command.Parameters.Add("@BookId", SqlDbType.Int).Value =
                                bookId;

                            if (Convert.ToInt32(command.ExecuteScalar()) > 0)
                            {
                                throw new InvalidOperationException(
                                    "Member has already borrowed this book.");
                            }
                        }

                        // Prepare the borrow and due dates
                        DateTime borrowDate =
                            DateRules.TrimSeconds(DateRules.Now());

                        DateTime dueDate =
                            DateRules.DefaultDueDate(borrowDate);

                        // Insert the borrow record
                        string insertQuery = @"
                            INSERT INTO Borrows
                                (BookId, MemberId, BorrowDate, DueDate, ReturnDate)
                            OUTPUT INSERTED.BorrowId
                            VALUES
                                (@BookId, @MemberId, @BorrowDate, @DueDate, NULL);";

                        int newBorrowId;

                        using (SqlCommand command =
                               new SqlCommand(insertQuery, connection, transaction))
                        {
                            command.Parameters.Add("@BookId", SqlDbType.Int).Value =
                                bookId;

                            command.Parameters.Add("@MemberId", SqlDbType.Int).Value =
                                memberId;

                            command.Parameters.Add("@BorrowDate", SqlDbType.DateTime2).Value =
                                borrowDate;

                            command.Parameters.Add("@DueDate", SqlDbType.DateTime2).Value =
                                dueDate;

                            newBorrowId =
                                Convert.ToInt32(command.ExecuteScalar());
                        }

                        // Reduce available book copies
                        string updateBookQuery = @"
                            UPDATE Books
                            SET AvailableCopies = AvailableCopies - 1
                            WHERE BookId = @BookId
                              AND AvailableCopies > 0;";

                        using (SqlCommand command =
                               new SqlCommand(updateBookQuery, connection, transaction))
                        {
                            command.Parameters.Add("@BookId", SqlDbType.Int).Value =
                                bookId;

                            if (command.ExecuteNonQuery() != 1)
                            {
                                throw new InvalidOperationException(
                                    "Unable to update book availability.");
                            }
                        }

                        transaction.Commit();

                        return newBorrowId;
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        // Return a borrowed book and create a fine if it is overdue
        public void ReturnBook(int borrowId, DateTime returnDate)
        {
            returnDate = DateRules.TrimSeconds(returnDate);

            if (returnDate.Date > DateTime.Today)
            {
                throw new ArgumentException(
                    "Return date cannot be in the future.");
            }

            using (SqlConnection connection = DatabaseHelper.GetConnection())
            {
                connection.Open();

                using (SqlTransaction transaction =
                       connection.BeginTransaction(IsolationLevel.Serializable))
                {
                    try
                    {
                        // Get the borrow record and lock it
                        string borrowQuery = @"
                            SELECT BookId, BorrowDate, DueDate, ReturnDate
                            FROM Borrows WITH (UPDLOCK, HOLDLOCK)
                            WHERE BorrowId = @BorrowId;";

                        int bookId;
                        DateTime borrowDate;
                        DateTime dueDate;

                        using (SqlCommand command =
                               new SqlCommand(borrowQuery, connection, transaction))
                        {
                            command.Parameters.Add("@BorrowId", SqlDbType.Int).Value =
                                borrowId;

                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                if (!reader.Read())
                                {
                                    throw new InvalidOperationException(
                                        "Borrow record not found.");
                                }

                                bookId = reader.GetInt32(0);
                                borrowDate = reader.GetDateTime(1);
                                dueDate = reader.GetDateTime(2);

                                if (!reader.IsDBNull(3))
                                {
                                    throw new InvalidOperationException(
                                        "This book has already been returned.");
                                }
                            }
                        }

                        if (returnDate < borrowDate)
                        {
                            throw new ArgumentException(
                                "Return date cannot be before the borrow date.");
                        }

                        // Update the return date
                        string updateBorrowQuery = @"
                            UPDATE Borrows
                            SET ReturnDate = @ReturnDate
                            WHERE BorrowId = @BorrowId
                              AND ReturnDate IS NULL;";

                        using (SqlCommand command =
                               new SqlCommand(updateBorrowQuery, connection, transaction))
                        {
                            command.Parameters.Add("@ReturnDate", SqlDbType.DateTime2).Value =
                                returnDate;

                            command.Parameters.Add("@BorrowId", SqlDbType.Int).Value =
                                borrowId;

                            if (command.ExecuteNonQuery() != 1)
                            {
                                throw new InvalidOperationException(
                                    "Unable to return this book.");
                            }
                        }

                        // Increase available book copies
                        string updateBookQuery = @"
                            UPDATE Books
                            SET AvailableCopies = AvailableCopies + 1
                            WHERE BookId = @BookId
                              AND AvailableCopies < TotalCopies;";

                        using (SqlCommand command =
                               new SqlCommand(updateBookQuery, connection, transaction))
                        {
                            command.Parameters.Add("@BookId", SqlDbType.Int).Value =
                                bookId;

                            if (command.ExecuteNonQuery() != 1)
                            {
                                throw new InvalidOperationException(
                                    "Unable to update book availability.");
                            }
                        }

                        // Calculate overdue days and fine
                        int overdueDays =
                            DateRules.CalculateOverdueDays(dueDate, returnDate);

                        if (overdueDays > 0)
                        {
                            decimal fineAmount =
                                DateRules.CalculateFine(overdueDays);

                            string fineQuery = @"
                                INSERT INTO Fines
                                    (BorrowId, OverdueDays, DailyRate,
                                     FineAmount, AmountPaid, Status,
                                     CreatedAt, LastPaymentAt)
                                VALUES
                                    (@BorrowId, @OverdueDays, @DailyRate,
                                     @FineAmount, 0, N'Unpaid',
                                     SYSDATETIME(), NULL);";

                            using (SqlCommand command =
                                   new SqlCommand(fineQuery, connection, transaction))
                            {
                                command.Parameters.Add("@BorrowId", SqlDbType.Int).Value =
                                    borrowId;

                                command.Parameters.Add("@OverdueDays", SqlDbType.Int).Value =
                                    overdueDays;

                                DatabaseHelper.AddMoney(
                                    command, "@DailyRate",
                                    LibrarySettings.DailyFineRate);

                                DatabaseHelper.AddMoney(
                                    command, "@FineAmount", fineAmount);

                                command.ExecuteNonQuery();
                            }
                        }

                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }
    }
}