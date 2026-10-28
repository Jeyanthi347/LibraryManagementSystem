using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using LibraryManagementSystem.Helpers;
using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Repositories
{
    public class MemberRepository
    {
        // Get all members
        public List<Member> GetAll()
        {
            List<Member> members = new List<Member>();

            using SqlConnection connection = DatabaseHelper.GetConnection();
            connection.Open();

            string query = @"
                SELECT MemberId, FullName, Email, Phone,
                       MembershipStartDate, MembershipExpiryDate, IsActive
                FROM Members
                ORDER BY MemberId";

            using SqlCommand command = new SqlCommand(query, connection);
            using SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                Member member = new Member(
                    reader.GetInt32(0),
                    reader.GetString(1),
                    reader.GetString(2),
                    reader.IsDBNull(3) ? null : reader.GetString(3),
                    DateOnly.FromDateTime(reader.GetDateTime(4)),
                    DateOnly.FromDateTime(reader.GetDateTime(5)),
                    reader.GetBoolean(6)
                );

                members.Add(member);
            }

            return members;
        }

        // Get one member by ID
        public Member? GetById(int memberId)
        {
            using SqlConnection connection = DatabaseHelper.GetConnection();
            connection.Open();

            string query = @"
                SELECT MemberId, FullName, Email, Phone,
                       MembershipStartDate, MembershipExpiryDate, IsActive
                FROM Members
                WHERE MemberId = @MemberId";

            using SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.Add("@MemberId", SqlDbType.Int).Value = memberId;

            using SqlDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                return new Member(
                    reader.GetInt32(0),
                    reader.GetString(1),
                    reader.GetString(2),
                    reader.IsDBNull(3) ? null : reader.GetString(3),
                    DateOnly.FromDateTime(reader.GetDateTime(4)),
                    DateOnly.FromDateTime(reader.GetDateTime(5)),
                    reader.GetBoolean(6)
                );
            }

            return null;
        }

        // Add a new member
        public int Add(Member member)
        {
            using SqlConnection connection = DatabaseHelper.GetConnection();
            connection.Open();

            string query = @"
                INSERT INTO Members
                (
                    FullName, Email, Phone,
                    MembershipStartDate, MembershipExpiryDate, IsActive
                )
                OUTPUT INSERTED.MemberId
                VALUES
                (
                    @FullName, @Email, @Phone,
                    @MembershipStartDate, @MembershipExpiryDate, @IsActive
                )";

            using SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.Add("@FullName", SqlDbType.NVarChar, 100)
                .Value = member.FullName;

            command.Parameters.Add("@Email", SqlDbType.NVarChar, 255)
                .Value = member.Email;

            command.Parameters.Add("@Phone", SqlDbType.NVarChar, 20)
                .Value = (object?)member.Phone ?? System.DBNull.Value;

            command.Parameters.Add("@MembershipStartDate", SqlDbType.Date)
                .Value = member.MembershipStartDate.ToDateTime(TimeOnly.MinValue);

            command.Parameters.Add("@MembershipExpiryDate", SqlDbType.Date)
                .Value = member.MembershipExpiryDate.ToDateTime(TimeOnly.MinValue);

            command.Parameters.Add("@IsActive", SqlDbType.Bit)
                .Value = member.IsActive;

            return (int)command.ExecuteScalar()!;
        }

        // Update member details
        public bool Update(Member member)
        {
            using SqlConnection connection = DatabaseHelper.GetConnection();
            connection.Open();

            string query = @"
                UPDATE Members
                SET FullName = @FullName,
                    Email = @Email,
                    Phone = @Phone,
                    MembershipStartDate = @MembershipStartDate,
                    MembershipExpiryDate = @MembershipExpiryDate,
                    IsActive = @IsActive
                WHERE MemberId = @MemberId";

            using SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.Add("@MemberId", SqlDbType.Int)
                .Value = member.MemberId;

            command.Parameters.Add("@FullName", SqlDbType.NVarChar, 100)
                .Value = member.FullName;

            command.Parameters.Add("@Email", SqlDbType.NVarChar, 255)
                .Value = member.Email;

            command.Parameters.Add("@Phone", SqlDbType.NVarChar, 20)
                .Value = (object?)member.Phone ?? System.DBNull.Value;

            command.Parameters.Add("@MembershipStartDate", SqlDbType.Date)
                .Value = member.MembershipStartDate.ToDateTime(TimeOnly.MinValue);

            command.Parameters.Add("@MembershipExpiryDate", SqlDbType.Date)
                .Value = member.MembershipExpiryDate.ToDateTime(TimeOnly.MinValue);

            command.Parameters.Add("@IsActive", SqlDbType.Bit)
                .Value = member.IsActive;

            return command.ExecuteNonQuery() > 0;
        }

        // Deactivate a member only if they have no active borrows
        public bool Deactivate(int memberId)
        {
            using SqlConnection connection = DatabaseHelper.GetConnection();
            connection.Open();

            string query = @"
                UPDATE Members
                SET IsActive = 0
                WHERE MemberId = @MemberId
                  AND NOT EXISTS
                  (
                      SELECT 1
                      FROM Borrows
                      WHERE Borrows.MemberId = Members.MemberId
                        AND Borrows.ReturnDate IS NULL
                  )";

            using SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.Add("@MemberId", SqlDbType.Int).Value = memberId;

            return command.ExecuteNonQuery() > 0;
        }
    }
}