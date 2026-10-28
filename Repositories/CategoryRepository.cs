using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using LibraryManagementSystem.Helpers;
using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Repositories
{
    public class CategoryRepository
    {
        // Get all categories
        public List<Category> GetAll()
        {
            List<Category> categories = new List<Category>();

            using SqlConnection connection = DatabaseHelper.GetConnection();
            connection.Open();

            string query = @"
                SELECT CategoryId, CategoryName, Description
                FROM Categories
                ORDER BY CategoryId";

            using SqlCommand command = new SqlCommand(query, connection);
            using SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                Category category = new Category(
                    reader.GetInt32(0),
                    reader.GetString(1),
                    reader.IsDBNull(2) ? null : reader.GetString(2)
                );

                categories.Add(category);
            }

            return categories;
        }

        // Get one category by ID
        public Category? GetById(int categoryId)
        {
            using SqlConnection connection = DatabaseHelper.GetConnection();
            connection.Open();

            string query = @"
                SELECT CategoryId, CategoryName, Description
                FROM Categories
                WHERE CategoryId = @CategoryId";

            using SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.Add("@CategoryId", SqlDbType.Int).Value = categoryId;

            using SqlDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                return new Category(
                    reader.GetInt32(0),
                    reader.GetString(1),
                    reader.IsDBNull(2) ? null : reader.GetString(2)
                );
            }

            return null;
        }

        // Add a new category
        public int Add(Category category)
        {
            using SqlConnection connection = DatabaseHelper.GetConnection();
            connection.Open();

            string query = @"
                INSERT INTO Categories (CategoryName, Description)
                OUTPUT INSERTED.CategoryId
                VALUES (@CategoryName, @Description)";

            using SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.Add("@CategoryName", SqlDbType.NVarChar, 60)
                .Value = category.CategoryName;

            command.Parameters.Add("@Description", SqlDbType.NVarChar, 250)
                .Value = (object?)category.Description ?? System.DBNull.Value;

            int newCategoryId = (int)command.ExecuteScalar()!;

            return newCategoryId;
        }

        // Update an existing category
        public bool Update(Category category)
        {
            using SqlConnection connection = DatabaseHelper.GetConnection();
            connection.Open();

            string query = @"
                UPDATE Categories
                SET CategoryName = @CategoryName,
                    Description = @Description
                WHERE CategoryId = @CategoryId";

            using SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.Add("@CategoryId", SqlDbType.Int)
                .Value = category.CategoryId;

            command.Parameters.Add("@CategoryName", SqlDbType.NVarChar, 60)
                .Value = category.CategoryName;

            command.Parameters.Add("@Description", SqlDbType.NVarChar, 250)
                .Value = (object?)category.Description ?? System.DBNull.Value;

            int rowsAffected = command.ExecuteNonQuery();

            return rowsAffected > 0;
        }

        // Delete only if no books reference this category
        public bool Delete(int categoryId)
        {
            using SqlConnection connection = DatabaseHelper.GetConnection();
            connection.Open();

            string query = @"
                DELETE FROM Categories
                WHERE CategoryId = @CategoryId
                  AND NOT EXISTS
                  (
                      SELECT 1
                      FROM Books
                      WHERE Books.CategoryId = Categories.CategoryId
                  )";

            using SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.Add("@CategoryId", SqlDbType.Int)
                .Value = categoryId;

            int rowsAffected = command.ExecuteNonQuery();

            return rowsAffected > 0;
        }
    }
}