using LibraryManagementSystem.Helpers;
using LibraryManagementSystem.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace LibraryManagementSystem.Repositories
{
    public class ReportRepository
    {
        // Report 1: Display book inventory
        public List<BookViewRow> GetInventoryReport()
        {
            List<BookViewRow> books = new List<BookViewRow>();

            using (SqlConnection connection = DatabaseHelper.GetConnection())
            {
                connection.Open();

                string query = @"
                    SELECT
                        b.BookId,
                        b.Title,
                        a.FullName AS AuthorName,
                        c.CategoryName,
                        b.TotalCopies,
                        b.AvailableCopies,
                        b.IsActive
                    FROM Books b
                    INNER JOIN Authors a ON b.AuthorId = a.AuthorId
                    INNER JOIN Categories c ON b.CategoryId = c.CategoryId
                    ORDER BY b.Title;";

                using (SqlCommand command = new SqlCommand(query, connection))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        BookViewRow row = new BookViewRow();

                        row.BookId = reader.GetInt32(0);
                        row.Title = reader.GetString(1);
                        row.AuthorName = reader.GetString(2);
                        row.CategoryName = reader.GetString(3);
                        row.TotalCopies = reader.GetInt32(4);
                        row.AvailableCopies = reader.GetInt32(5);
                        row.IsActive = reader.GetBoolean(6);

                        books.Add(row);
                    }
                }
            }

            return books;
        }

        // Report 2: Display complete borrowing history
        public List<BorrowViewRow> GetBorrowHistoryReport()
        {
            List<BorrowViewRow> borrows = new List<BorrowViewRow>();

            using (SqlConnection connection = DatabaseHelper.GetConnection())
            {
                connection.Open();

                string query = @"
                    SELECT
                        br.BorrowId,
                        b.Title AS BookTitle,
                        m.FullName AS MemberName,
                        br.BorrowDate,
                        br.DueDate,
                        br.ReturnDate
                    FROM Borrows br
                    INNER JOIN Books b ON br.BookId = b.BookId
                    INNER JOIN Members m ON br.MemberId = m.MemberId
                    ORDER BY br.BorrowDate DESC;";

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

        // Report 3: Display books that are currently overdue
        public List<BorrowViewRow> GetOverdueReport()
        {
            List<BorrowViewRow> borrows = new List<BorrowViewRow>();

            using (SqlConnection connection = DatabaseHelper.GetConnection())
            {
                connection.Open();

                string query = @"
                    SELECT
                        br.BorrowId,
                        b.Title AS BookTitle,
                        m.FullName AS MemberName,
                        br.BorrowDate,
                        br.DueDate,
                        br.ReturnDate
                    FROM Borrows br
                    INNER JOIN Books b ON br.BookId = b.BookId
                    INNER JOIN Members m ON br.MemberId = m.MemberId
                    WHERE br.ReturnDate IS NULL
                      AND CAST(GETDATE() AS DATE) > CAST(br.DueDate AS DATE)
                    ORDER BY br.DueDate;";

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

        // Report 4: Display fine details
        public List<FineViewRow> GetFineReport()
        {
            List<FineViewRow> fines = new List<FineViewRow>();

            using (SqlConnection connection = DatabaseHelper.GetConnection())
            {
                connection.Open();

                string query = @"
                    SELECT
                        f.FineId,
                        f.BorrowId,
                        m.FullName AS MemberName,
                        b.Title AS BookTitle,
                        f.OverdueDays,
                        f.DailyRate,
                        f.FineAmount,
                        f.AmountPaid,
                        f.Status,
                        f.CreatedAt
                    FROM Fines f
                    INNER JOIN Borrows br ON f.BorrowId = br.BorrowId
                    INNER JOIN Members m ON br.MemberId = m.MemberId
                    INNER JOIN Books b ON br.BookId = b.BookId
                    ORDER BY f.CreatedAt DESC;";

                using (SqlCommand command = new SqlCommand(query, connection))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        FineViewRow row = new FineViewRow();

                        row.FineId = reader.GetInt32(0);
                        row.BorrowId = reader.GetInt32(1);
                        row.MemberName = reader.GetString(2);
                        row.BookTitle = reader.GetString(3);
                        row.OverdueDays = reader.GetInt32(4);
                        row.DailyRate = reader.GetDecimal(5);
                        row.FineAmount = reader.GetDecimal(6);
                        row.AmountPaid = reader.GetDecimal(7);
                        row.Status = reader.GetString(8);
                        row.CreatedAt = reader.GetDateTime(9);

                        fines.Add(row);
                    }
                }
            }

            return fines;
        }
    }
}