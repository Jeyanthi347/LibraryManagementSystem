using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using LibraryManagementSystem.Helpers;
using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Repositories
{
    public class BookRepository
    {
        // Get all books with author and category names
        public List<BookViewRow> GetAll()
        {
            List<BookViewRow> books = new List<BookViewRow>();

            using SqlConnection connection = DatabaseHelper.GetConnection();
            connection.Open();

            string query = @"
                SELECT
                    b.BookId,
                    b.Title,
                    b.Isbn,
                    a.FullName AS AuthorName,
                    c.CategoryName,
                    b.TotalCopies,
                    b.AvailableCopies
                FROM Books b
                INNER JOIN Authors a ON b.AuthorId = a.AuthorId
                INNER JOIN Categories c ON b.CategoryId = c.CategoryId
                WHERE b.IsActive = 1
                ORDER BY b.Title";

            using SqlCommand command = new SqlCommand(query, connection);
            using SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                BookViewRow book = new BookViewRow
                {
                    BookId = reader.GetInt32(0),
                    Title = reader.GetString(1),
                    Isbn = reader.GetString(2),
                    AuthorName = reader.GetString(3),
                    CategoryName = reader.GetString(4),
                    TotalCopies = reader.GetInt32(5),
                    AvailableCopies = reader.GetInt32(6)
                };

                books.Add(book);
            }

            return books;
        }

        // Get one book by ID
        public Book? GetById(int bookId)
        {
            using SqlConnection connection = DatabaseHelper.GetConnection();
            connection.Open();

            string query = @"
                SELECT BookId, Title, Isbn, AuthorId, CategoryId,
                       PublicationYear, TotalCopies, AvailableCopies, IsActive
                FROM Books
                WHERE BookId = @BookId";

            using SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.Add("@BookId", SqlDbType.Int).Value = bookId;

            using SqlDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                return new Book(
                    reader.GetInt32(0),
                    reader.GetString(1),
                    reader.GetString(2),
                    reader.GetInt32(3),
                    reader.GetInt32(4),
                    reader.IsDBNull(5) ? null : reader.GetInt32(5),
                    reader.GetInt32(6),
                    reader.GetInt32(7),
                    reader.GetBoolean(8)
                );
            }

            return null;
        }

        // Add a new book
        public int Add(Book book)
        {
            using SqlConnection connection = DatabaseHelper.GetConnection();
            connection.Open();

            string query = @"
                INSERT INTO Books
                (
                    Title, Isbn, AuthorId, CategoryId,
                    PublicationYear, TotalCopies, AvailableCopies, IsActive
                )
                OUTPUT INSERTED.BookId
                VALUES
                (
                    @Title, @Isbn, @AuthorId, @CategoryId,
                    @PublicationYear, @TotalCopies, @AvailableCopies, @IsActive
                )";

            using SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.Add("@Title", SqlDbType.NVarChar, 200).Value =
                book.Title;

            command.Parameters.Add("@Isbn", SqlDbType.NVarChar, 20).Value =
                book.Isbn;

            command.Parameters.Add("@AuthorId", SqlDbType.Int).Value =
                book.AuthorId;

            command.Parameters.Add("@CategoryId", SqlDbType.Int).Value =
                book.CategoryId;

            command.Parameters.Add("@PublicationYear", SqlDbType.Int).Value =
                (object?)book.PublicationYear ?? System.DBNull.Value;

            command.Parameters.Add("@TotalCopies", SqlDbType.Int).Value =
                book.TotalCopies;

            command.Parameters.Add("@AvailableCopies", SqlDbType.Int).Value =
                book.AvailableCopies;

            command.Parameters.Add("@IsActive", SqlDbType.Bit).Value =
                book.IsActive;

            return (int)command.ExecuteScalar()!;
        }

        // Update an existing book
        public bool Update(Book book)
        {
            using SqlConnection connection = DatabaseHelper.GetConnection();
            connection.Open();

            string query = @"
                UPDATE Books
                SET Title = @Title,
                    Isbn = @Isbn,
                    AuthorId = @AuthorId,
                    CategoryId = @CategoryId,
                    PublicationYear = @PublicationYear,
                    TotalCopies = @TotalCopies,
                    AvailableCopies = @AvailableCopies,
                    IsActive = @IsActive
                WHERE BookId = @BookId";

            using SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.Add("@BookId", SqlDbType.Int).Value =
                book.BookId;

            command.Parameters.Add("@Title", SqlDbType.NVarChar, 200).Value =
                book.Title;

            command.Parameters.Add("@Isbn", SqlDbType.NVarChar, 20).Value =
                book.Isbn;

            command.Parameters.Add("@AuthorId", SqlDbType.Int).Value =
                book.AuthorId;

            command.Parameters.Add("@CategoryId", SqlDbType.Int).Value =
                book.CategoryId;

            command.Parameters.Add("@PublicationYear", SqlDbType.Int).Value =
                (object?)book.PublicationYear ?? System.DBNull.Value;

            command.Parameters.Add("@TotalCopies", SqlDbType.Int).Value =
                book.TotalCopies;

            command.Parameters.Add("@AvailableCopies", SqlDbType.Int).Value =
                book.AvailableCopies;

            command.Parameters.Add("@IsActive", SqlDbType.Bit).Value =
                book.IsActive;

            return command.ExecuteNonQuery() > 0;
        }

        // Delete a book if it has no borrow history.
        // Otherwise, deactivate it instead of deleting its history.
        public bool Delete(int bookId)
        {
            using SqlConnection connection = DatabaseHelper.GetConnection();
            connection.Open();

            using SqlTransaction transaction = connection.BeginTransaction();

            try
            {
                string historyQuery = @"
                    SELECT COUNT(*)
                    FROM Borrows
                    WHERE BookId = @BookId";

                int borrowCount;

                using (SqlCommand command =
                    new SqlCommand(historyQuery, connection, transaction))
                {
                    command.Parameters.Add("@BookId", SqlDbType.Int).Value =
                        bookId;

                    borrowCount = (int)command.ExecuteScalar()!;
                }

                string query;

                if (borrowCount > 0)
                {
                    query = @"
                        UPDATE Books
                        SET IsActive = 0
                        WHERE BookId = @BookId";
                }
                else
                {
                    query = @"
                        DELETE FROM Books
                        WHERE BookId = @BookId";
                }

                int rowsAffected;

                using (SqlCommand command =
                    new SqlCommand(query, connection, transaction))
                {
                    command.Parameters.Add("@BookId", SqlDbType.Int).Value =
                        bookId;

                    rowsAffected = command.ExecuteNonQuery();
                }

                transaction.Commit();

                return rowsAffected > 0;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
    }
}