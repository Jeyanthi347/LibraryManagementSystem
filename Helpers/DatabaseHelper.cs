using Microsoft.Data.SqlClient;
using System.Data;

namespace LibraryManagementSystem.Helpers
{
    public static class DatabaseHelper
    {
        // Connection string for SQL Server Express
        private static readonly string connectionString =
            @"Server=.\SQLEXPRESS;Database=LibraryDB;Trusted_Connection=True;TrustServerCertificate=True;";

        // Create and return a SQL Server connection
        public static SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }

        // Add a decimal parameter for money values
        public static void AddMoney(
            SqlCommand command,
            string parameterName,
            decimal amount)
        {
            SqlParameter parameter =
                command.Parameters.Add(parameterName, SqlDbType.Decimal);

            parameter.Precision = 10;
            parameter.Scale = 2;
            parameter.Value = amount;
        }
    }
}