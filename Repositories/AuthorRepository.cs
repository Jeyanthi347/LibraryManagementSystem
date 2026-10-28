using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using LibraryManagementSystem.Helpers;
using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Repositories
{
    public class AuthorRepository
    {
        // Get all authors
        public List<Author> GetAll()
        {
            List<Author> authors = new List<Author>();

            using SqlConnection connection = DatabaseHelper.GetConnection();
            connection.Open();

            string query = @"
                SELECT AuthorId, FullName, Country
                FROM Authors
                ORDER BY AuthorId";

            using SqlCommand command = new SqlCommand(query, connection);
            using SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                Author author = new Author(
                    reader.GetInt32(0),
                    reader.GetString(1),
                    reader.IsDBNull(2) ? null : reader.GetString(2)
                );

                authors.Add(author);
            }

            return authors;
        }

        // Get one author by ID
        public Author? GetById(int authorId)
        {
            using SqlConnection connection = DatabaseHelper.GetConnection();
            connection.Open();

            string query = @"
                SELECT AuthorId, FullName, Country
                FROM Authors
                WHERE AuthorId = @AuthorId";

            using SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@AuthorId", authorId);

            using SqlDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                return new Author(
                    reader.GetInt32(0),
                    reader.GetString(1),
                    reader.IsDBNull(2) ? null : reader.GetString(2)
                );
            }

            return null;
        }

        // Add a new author
        public int Add(Author author)
        {
            using SqlConnection connection = DatabaseHelper.GetConnection();
            connection.Open();

            string query = @"
                INSERT INTO Authors (FullName, Country)
                OUTPUT INSERTED.AuthorId
                VALUES (@FullName, @Country)";

            using SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@FullName", author.FullName);

            command.Parameters.Add("@Country", System.Data.SqlDbType.NVarChar, 60)
                .Value = (object?)author.Country ?? System.DBNull.Value;

            int newAuthorId = (int)command.ExecuteScalar()!;

            return newAuthorId;
        }

        // Update an existing author
        public bool Update(Author author)
        {
            using SqlConnection connection = DatabaseHelper.GetConnection();
            connection.Open();

            string query = @"
                UPDATE Authors
                SET FullName = @FullName,
                    Country = @Country
                WHERE AuthorId = @AuthorId";

            using SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@AuthorId", author.AuthorId);
            command.Parameters.AddWithValue("@FullName", author.FullName);

            command.Parameters.Add("@Country", System.Data.SqlDbType.NVarChar, 60)
                .Value = (object?)author.Country ?? System.DBNull.Value;

            int rowsAffected = command.ExecuteNonQuery();

            return rowsAffected > 0;
        }

        // Delete an author only if no books reference the author
        public bool Delete(int authorId)
        {
            using SqlConnection connection = DatabaseHelper.GetConnection();
            connection.Open();

            string query = @"
                DELETE FROM Authors
                WHERE AuthorId = @AuthorId
                  AND NOT EXISTS
                  (
                      SELECT 1
                      FROM Books
                      WHERE Books.AuthorId = Authors.AuthorId
                  )";

            using SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@AuthorId", authorId);

            int rowsAffected = command.ExecuteNonQuery();

            return rowsAffected > 0;
        }
    }
}