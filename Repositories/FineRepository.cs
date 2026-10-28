using LibraryManagementSystem.Helpers;
using LibraryManagementSystem.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace LibraryManagementSystem.Repositories
{
    public class FineRepository
    {
        // Display all fine records
        public List<FineViewRow> GetAll()
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
                    ORDER BY f.FineId DESC;";

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

        // Find one fine record by its ID
        public Fine? GetById(int fineId)
        {
            using (SqlConnection connection = DatabaseHelper.GetConnection())
            {
                connection.Open();

                string query = @"
                    SELECT FineId, BorrowId, OverdueDays, DailyRate,
                           FineAmount, AmountPaid, Status,
                           CreatedAt, LastPaymentAt
                    FROM Fines
                    WHERE FineId = @FineId;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@FineId", SqlDbType.Int).Value =
                        fineId;

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Fine fine = new Fine();

                            fine.FineId = reader.GetInt32(0);
                            fine.BorrowId = reader.GetInt32(1);
                            fine.OverdueDays = reader.GetInt32(2);
                            fine.DailyRate = reader.GetDecimal(3);
                            fine.FineAmount = reader.GetDecimal(4);
                            fine.AmountPaid = reader.GetDecimal(5);
                            fine.Status = reader.GetString(6);
                            fine.CreatedAt = reader.GetDateTime(7);

                            fine.LastPaymentAt = reader.IsDBNull(8)
                                ? null
                                : reader.GetDateTime(8);

                            return fine;
                        }
                    }
                }
            }

            return null;
        }

        // Record a payment against a fine
        public void PayFine(int fineId, decimal paymentAmount)
        {
            if (paymentAmount <= 0)
            {
                throw new ArgumentException(
                    "Payment amount must be greater than zero.");
            }

            using (SqlConnection connection = DatabaseHelper.GetConnection())
            {
                connection.Open();

                using (SqlTransaction transaction =
                       connection.BeginTransaction(IsolationLevel.Serializable))
                {
                    try
                    {
                        string selectQuery = @"
                            SELECT FineAmount, AmountPaid
                            FROM Fines WITH (UPDLOCK, HOLDLOCK)
                            WHERE FineId = @FineId;";

                        decimal fineAmount;
                        decimal amountPaid;

                        using (SqlCommand command =
                               new SqlCommand(selectQuery, connection, transaction))
                        {
                            command.Parameters.Add("@FineId", SqlDbType.Int).Value =
                                fineId;

                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                if (!reader.Read())
                                {
                                    throw new InvalidOperationException(
                                        "Fine record not found.");
                                }

                                fineAmount = reader.GetDecimal(0);
                                amountPaid = reader.GetDecimal(1);
                            }
                        }

                        decimal remainingAmount = fineAmount - amountPaid;

                        if (remainingAmount <= 0)
                        {
                            throw new InvalidOperationException(
                                "This fine has already been fully paid.");
                        }

                        if (paymentAmount > remainingAmount)
                        {
                            throw new ArgumentException(
                                "Payment cannot exceed the remaining fine amount.");
                        }

                        decimal newAmountPaid = amountPaid + paymentAmount;

                        string newStatus =
                            newAmountPaid >= fineAmount ? "Paid" : "Unpaid";

                        string updateQuery = @"
                            UPDATE Fines
                            SET AmountPaid = @AmountPaid,
                                Status = @Status,
                                LastPaymentAt = SYSDATETIME()
                            WHERE FineId = @FineId;";

                        using (SqlCommand command =
                               new SqlCommand(updateQuery, connection, transaction))
                        {
                            DatabaseHelper.AddMoney(
                                command, "@AmountPaid", newAmountPaid);

                            command.Parameters.Add("@Status", SqlDbType.NVarChar, 10).Value =
                                newStatus;

                            command.Parameters.Add("@FineId", SqlDbType.Int).Value =
                                fineId;

                            command.ExecuteNonQuery();
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